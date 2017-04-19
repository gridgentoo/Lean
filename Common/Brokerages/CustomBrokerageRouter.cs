using System;
using System.Collections.Generic;
using System.Linq;
using QuantConnect.Data;
using QuantConnect.Orders;

namespace QuantConnect.Brokerages
{
    /// <summary>
    /// Provides a skeleton implementation of <see cref="IBrokerageRouter"/> that
    /// can be extended by user algorithms.
    /// </summary>
    public abstract class CustomBrokerageRouter : IBrokerageRouter
    {
        private readonly BrokerageSpecifier[] _specifiers;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomBrokerageRouter"/> class
        /// </summary>
        /// <param name="specifiers">The brokerages loaded by the algorithm</param>
        protected CustomBrokerageRouter(IEnumerable<BrokerageSpecifier> specifiers)
        {
            _specifiers = specifiers.ToArray();
        }

        /// <summary>
        /// Determines which brokerage should execute the order
        /// </summary>
        public BrokerageSpecifier RouteOrder(IEnumerable<BrokerageSpecifier> brokerages, Order order)
        {
            return GetBrokerage(Route(order));
        }

        /// <summary>
        /// Determines which brokerage should execute the history request
        /// </summary>
        public BrokerageSpecifier RouteHistoryRequest(IEnumerable<BrokerageSpecifier> brokerages, HistoryRequest request)
        {
            return GetBrokerage(Route(request));
        }

        /// <summary>
        /// Determines which brokerage should execute the order
        /// </summary>
        /// <param name="order">The order to be routed</param>
        /// <returns>The <see cref="BrokerageName"/> of the brokerage to execute the order</returns>
        public abstract BrokerageName Route(Order order);

        /// <summary>
        /// Determines which brokerage should execute the history request
        /// </summary>
        /// <param name="request">The history request to be routed</param>
        /// <returns>The <see cref="BrokerageName"/> of the brokerage to execute the history request</returns>
        public abstract BrokerageName Route(HistoryRequest request);

        private BrokerageSpecifier GetBrokerage(BrokerageName brokerage)
        {
            try
            {
                return _specifiers.Single(specifier => specifier.Name == brokerage);
            }
            catch (InvalidOperationException err)
            {
                // give the user a better error message if _brokerages doesn't contain the requested brokerage
                throw new Exception("The specified brokerage is not loaded: " + brokerage, err);
            }
        }
    }
}