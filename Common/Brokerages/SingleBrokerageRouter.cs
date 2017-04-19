using System.Collections.Generic;
using QuantConnect.Data;
using QuantConnect.Interfaces;
using QuantConnect.Orders;

namespace QuantConnect.Brokerages
{
    /// <summary>
    /// Provides an implementation of <see cref="IBrokerageRouter"/> that always
    /// routes to the same <see cref="IBrokerage"/> instance
    /// </summary>
    public class SingleBrokerageRouter : IBrokerageRouter
    {
        private readonly BrokerageSpecifier _brokerageSpecifier;

        /// <summary>
        /// Initializes a new instance of the <see cref="SingleBrokerageRouter"/> class
        /// </summary>
        /// <param name="brokerageSpecifier">The brokerage to always route to</param>
        public SingleBrokerageRouter(BrokerageSpecifier brokerageSpecifier)
        {
            _brokerageSpecifier = brokerageSpecifier;
        }

        /// <summary>
        /// Determines which brokerage should execute the order
        /// </summary>
        public BrokerageSpecifier RouteOrder(IEnumerable<BrokerageSpecifier> brokerages, Order order)
        {
            return _brokerageSpecifier;
        }

        /// <summary>
        /// Determines which brokerage should execute the history request
        /// </summary>
        public BrokerageSpecifier RouteHistoryRequest(IEnumerable<BrokerageSpecifier> brokerages, HistoryRequest request)
        {
            return _brokerageSpecifier;
        }
    }
}