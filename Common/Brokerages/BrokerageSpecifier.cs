namespace QuantConnect.Brokerages
{
    /// <summary>
    /// Provides access to details of a specific brokerage
    /// </summary>
    public class BrokerageSpecifier
    {
        /// <summary>
        /// Gets the brokerage account name
        /// </summary>
        public string Account { get; private set; }

        /// <summary>
        /// Gets the <see cref="BrokerageName"/>
        /// </summary>
        public BrokerageName Name { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BrokerageSpecifier"/> class
        /// </summary>
        /// <param name="name">The brokerage's name</param>
        /// <param name="account">The user's account id</param>
        public BrokerageSpecifier(BrokerageName name, string account)
        {
            Name = name;
            Account = account;
        }
    }
}
