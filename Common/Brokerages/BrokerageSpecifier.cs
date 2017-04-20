namespace QuantConnect.Brokerages
{
    /// <summary>
    /// Provides access to details of a specific brokerage
    /// </summary>
    public class BrokerageSpecifier
    {
        /// <summary>
        /// Gets the default brokerage specifier used by the default models and paper trading
        /// </summary>
        public static readonly BrokerageSpecifier Default = new BrokerageSpecifier(BrokerageName.Default, "paper");
        
        /// <summary>
        /// Gets the default FXCM brokerage specifer used when backtesting with FXCM model
        /// </summary>
        public static readonly BrokerageSpecifier Fxcm = new BrokerageSpecifier(BrokerageName.FxcmBrokerage, "backtesting-fxcm");
        
        /// <summary>
        /// Gets the default Oanda brokerage specifier used when backtesting with Oanda models
        /// </summary>
        public static readonly BrokerageSpecifier Oanda = new BrokerageSpecifier(BrokerageName.OandaBrokerage, "backtesting-oanda");
        
        /// <summary>
        /// Gets the default Tradier brokerage specifier used when backtesting with Tradier models
        /// </summary>
        public static readonly BrokerageSpecifier Tradier = new BrokerageSpecifier(BrokerageName.TradierBrokerage, "backtesting-tradier");
        
        /// <summary>
        /// Gets the default Interactive Brokerages brokerage specifier used when backtesting with Interactive Brokers models
        /// </summary>
        public static readonly BrokerageSpecifier Interactive = new BrokerageSpecifier(BrokerageName.InteractiveBrokersBrokerage, "backtesting-interactive");

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
