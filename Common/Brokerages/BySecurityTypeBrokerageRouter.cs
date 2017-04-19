using System.Collections.Generic;
using QuantConnect.Data;
using QuantConnect.Interfaces;
using QuantConnect.Orders;
using QuantConnect.Util;

namespace QuantConnect.Brokerages
{
    /// <summary>
    /// Provides an implementation of <see cref="IBrokerageRouter"/> that routes
    /// to brokerages based on <see cref="SecurityType"/>
    /// </summary>
    public class BySecurityTypeBrokerageRouter : IBrokerageRouter
    {
        private readonly Dictionary<SecurityType, BrokerageSpecifier> _brokerages;

        /// <summary>
        /// Initializes a new instance of the <see cref="BySecurityTypeBrokerageRouter"/> class
        /// </summary>
        /// <param name="brokerages">Map of security types to brokerage</param>
        public BySecurityTypeBrokerageRouter(Dictionary<SecurityType, BrokerageSpecifier> brokerages)
        {
            // make a copy to avoid the mutable reference
            _brokerages = brokerages.ToDictionary();
        }

        /// <summary>
        /// Determines which brokerage should execute the order
        /// </summary>
        public BrokerageSpecifier RouteOrder(IEnumerable<BrokerageSpecifier> brokerages, Order order)
        {
            return _brokerages[order.Symbol.SecurityType];
        }

        /// <summary>
        /// Determines which brokerage should execute the history request
        /// </summary>
        public BrokerageSpecifier RouteHistoryRequest(IEnumerable<BrokerageSpecifier> brokerages, HistoryRequest request)
        {
            return _brokerages[request.Symbol.SecurityType];
        }
    }
}