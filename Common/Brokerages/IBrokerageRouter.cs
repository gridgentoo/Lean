using System.Collections.Generic;
using QuantConnect.Data;
using QuantConnect.Orders;

namespace QuantConnect.Brokerages
{
    /// <summary>
    /// Represents the required routing logic when managing multiple brokerages via
    /// the CompositeBrokerage.
    /// </summary>
    public interface IBrokerageRouter
    {
        /// <summary>
        /// Determines which brokerage should execute the order
        /// </summary>
        BrokerageSpecifier RouteOrder(IEnumerable<BrokerageSpecifier> brokerages, Order order);

        /// <summary>
        /// Determines which brokerage should execute the history request
        /// </summary>
        BrokerageSpecifier RouteHistoryRequest(IEnumerable<BrokerageSpecifier> brokerages, HistoryRequest request);
    }
}