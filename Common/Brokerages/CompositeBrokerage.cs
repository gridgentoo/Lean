using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QuantConnect.Data;
using QuantConnect.Interfaces;
using QuantConnect.Orders;
using QuantConnect.Securities;

namespace QuantConnect.Brokerages
{
    /// <summary>
    /// Provides a 'brokerage of brokerages' implementation allowing the algorithm
    /// to execute seamlessly against multiple brokerages. This is aided by usage
    /// of an <see cref="IBrokerageRouter"/> instance to handle the logic of routing
    /// orders and various requests to a specific brokerage at runtime.
    /// </summary>
    public class CompositeBrokerage : IBrokerage
    {
        private readonly IBrokerage[] _brokerages;
        private readonly BrokerageSpecifier[] _specifiers;
        private readonly IBrokerageRouter _router;

        /// <summary>
        /// Event that fires each time an order is filled
        /// </summary>
        public event EventHandler<OrderEvent> OrderStatusChanged
        {
            add    { ForEach(brokerage => brokerage.OrderStatusChanged += value); }
            remove { ForEach(brokerage => brokerage.OrderStatusChanged -= value); }
        }

        /// <summary>
        /// Event that fires each time a short option position is assigned
        /// </summary>
        public event EventHandler<OrderEvent> OptionPositionAssigned
        {
            add    { ForEach(brokerage => brokerage.OptionPositionAssigned += value); }
            remove { ForEach(brokerage => brokerage.OptionPositionAssigned -= value); }
        }

        /// <summary>
        /// Event that fires each time a user's brokerage account is changed
        /// </summary>
        public event EventHandler<AccountEvent> AccountChanged
        {
            add    { ForEach(brokerage => brokerage.AccountChanged += value); }
            remove { ForEach(brokerage => brokerage.AccountChanged -= value); }
        }

        /// <summary>
        /// Event that fires when a message is received from the brokerage
        /// </summary>
        public event EventHandler<BrokerageMessageEvent> Message
        {
            add    { ForEach(brokerage => brokerage.Message += value); }
            remove { ForEach(brokerage => brokerage.Message -= value); }
        }

        /// <summary>
        /// Gets the name of the brokerage
        /// </summary>
        public string Name
        {
            get { return string.Join(" | ", _brokerages.Select(brokerage => brokerage.Name)); }
        }

        /// <summary>
        /// Gets the specifier for this brokerage instance
        /// </summary>
        public BrokerageSpecifier BrokerageSpecifier
        {
            get
            {
                var accounts = string.Join("; ", _brokerages.Select(brokerage => brokerage.BrokerageSpecifier.Account));
                return new BrokerageSpecifier(BrokerageName.Default, accounts);
            }
        }

        /// <summary>
        /// Returns true if we're currently connected to the broker
        /// </summary>
        public bool IsConnected
        {
            get { return _brokerages.All(brokerage => brokerage.IsConnected); }
        }

        /// <summary>
        /// Specifies whether the brokerage will instantly update account balances
        /// </summary>
        public bool AccountInstantlyUpdated
        {
            get { return _brokerages.All(brokerage => brokerage.AccountInstantlyUpdated); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBrokerage"/> class
        /// </summary>
        /// <param name="router">Used to route orders and requests to a specific brokerage</param>
        /// <param name="brokerages">The child brokerages</param>
        public CompositeBrokerage(IBrokerageRouter router, params IBrokerage[] brokerages)
            : this(router, (IEnumerable<IBrokerage>) brokerages)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBrokerage"/> class
        /// </summary>
        /// <param name="router">Used to route orders and requests to a specific brokerage</param>
        /// <param name="brokerages">The child brokerages</param>
        public CompositeBrokerage(IBrokerageRouter router, IEnumerable<IBrokerage> brokerages)
        {
            _router = router;
            _brokerages = brokerages.ToArray();
            _specifiers = _brokerages.Select(brokerage => brokerage.BrokerageSpecifier).ToArray();
        }

        /// <summary>
        /// Gets all open orders on the account
        /// </summary>
        /// <returns>The open orders returned from IB</returns>
        public List<Order> GetOpenOrders()
        {
            return ForEachAsync(brokerage => brokerage.GetOpenOrders())
                .SelectMany(oo => oo)
                .ToList();
        }

        /// <summary>
        /// Gets all holdings for the account
        /// </summary>
        /// <returns>The current holdings from the account</returns>
        public List<Holding> GetAccountHoldings()
        {
            return (
                from brokerageHoldings in ForEachAsync(brokerage => brokerage.GetAccountHoldings())
                from holding in brokerageHoldings
                group holding by holding.Symbol into grouping
                let quantity = grouping.Sum(h => h.Quantity)
                select new Holding
                {
                    Quantity = quantity,
                    Symbol = grouping.Key,
                    Type = grouping.Key.SecurityType,
                    CurrencySymbol = grouping.First().CurrencySymbol,

                    MarketValue = grouping.Sum(h => h.MarketValue),
                    MarketPrice = grouping.Sum(h => h.Quantity*h.MarketPrice)/quantity,
                    AveragePrice = grouping.Sum(h => h.Quantity*h.AveragePrice)/quantity,
                    ConversionRate = grouping.Sum(h => h.Quantity*h.ConversionRate)/quantity
                }
            ).ToList();
        }

        /// <summary>
        /// Gets the current cash balance for each currency held in the brokerage account
        /// </summary>
        /// <returns>The current cash balance for each currency available for trading</returns>
        public List<Cash> GetCashBalance()
        {
            return (
                from brokerageBalance in ForEachAsync(brokerage => brokerage.GetCashBalance())
                from balance in brokerageBalance
                group balance by balance.Symbol into grouping
                let amount = grouping.Sum(c => c.Amount)
                select new Cash(grouping.Key, amount,
                    grouping.Sum(c => c.Amount * c.ConversionRate) / amount
                )
            ).ToList();
        }

        /// <summary>
        /// Places a new order and assigns a new broker ID to the order
        /// </summary>
        /// <param name="order">The order to be placed</param>
        /// <returns>True if the request for a new order has been placed, false otherwise</returns>
        public bool PlaceOrder(Order order)
        {
            var specifier = _router.RouteOrder(_specifiers, order);
            var brokerage = GetBrokerage(specifier);
            return brokerage.PlaceOrder(order);
        }

        /// <summary>
        /// Updates the order with the same id
        /// </summary>
        /// <param name="order">The new order information</param>
        /// <returns>True if the request was made for the order to be updated, false otherwise</returns>
        public bool UpdateOrder(Order order)
        {
            var specifier = _router.RouteOrder(_specifiers, order);
            var brokerage = GetBrokerage(specifier);
            return brokerage.UpdateOrder(order);
        }

        /// <summary>
        /// Cancels the order with the specified ID
        /// </summary>
        /// <param name="order">The order to cancel</param>
        /// <returns>True if the request was made for the order to be canceled, false otherwise</returns>
        public bool CancelOrder(Order order)
        {
            var specifier = _router.RouteOrder(_specifiers, order);
            var brokerage = GetBrokerage(specifier);
            return brokerage.CancelOrder(order);
        }

        /// <summary>
        /// Connects the client to the broker's remote servers
        /// </summary>
        public void Connect()
        {
            ForEachAsync(brokerage => brokerage.Connect());
        }

        /// <summary>
        /// Disconnects the client from the broker's remote servers
        /// </summary>
        public void Disconnect()
        {
            ForEachAsync(brokerage => brokerage.Disconnect());
        }

        /// <summary>
        /// Gets the history for the requested security
        /// </summary>
        /// <param name="request">The historical data request</param>
        /// <returns>An enumerable of bars covering the span specified in the request</returns>
        public IEnumerable<BaseData> GetHistory(HistoryRequest request)
        {
            var specifier = _router.RouteHistoryRequest(_specifiers, request);
            var brokerage = GetBrokerage(specifier);
            return brokerage.GetHistory(request);
        }

        private void ForEach(Action<IBrokerage> action)
        {
            foreach (var brokerage in _brokerages)
            {
                action(brokerage);
            }
        }

        private void ForEachAsync(Action<IBrokerage> action)
        {
            ForEachAsync(brokerage =>
            {
                action(brokerage);
                return 0;
            });
        }

        private List<TResult> ForEachAsync<TResult>(Func<IBrokerage, TResult> func)
        {
            var tasks = new List<Task<TResult>>();

            foreach (var brokerage in _brokerages)
            {
                // avoid reference shenanigans in the delegate
                var localBrokerage = brokerage;
                tasks.Add(Task.Factory.StartNew(() => func(localBrokerage)));
            }

            var aggregate = Task.WhenAll(tasks).ContinueWith(_ => tasks.Select(t => t.Result).ToList());
            return aggregate.ConfigureAwait(false).GetAwaiter().GetResult();
        }

        private IBrokerage GetBrokerage(BrokerageSpecifier specifier)
        {
            return _brokerages.Single(brokerage => brokerage.BrokerageSpecifier == specifier);
        }
    }
}