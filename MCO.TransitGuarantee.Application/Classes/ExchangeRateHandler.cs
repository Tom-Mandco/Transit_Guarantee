namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Domain.Interfaces;
    using System;
    using System.Configuration;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Diagnostics;

    public class ExchangeRateHandler : IExchangeRateHandler
    {
        private readonly ILog logger;
        private readonly IPerformLookup performLookup;

        private readonly string exchangeRateExe;

        private readonly int retryAttempts;
        private readonly int timeoutMS;

        public ExchangeRateHandler(ILog logger, IPerformLookup performLookup)
        {
            this.logger = logger;
            this.performLookup = performLookup;

            int.TryParse(ConfigurationManager.AppSettings["ExchangeRateRetryAttempts"], out retryAttempts);
            int.TryParse(ConfigurationManager.AppSettings["ExchangeRateTimeoutMS"], out timeoutMS);

            exchangeRateExe = ConfigurationManager.AppSettings["ExchangeRateExePath"];
        }

        public void EnsureExchangeRatesAreCurrent()
        {
            if (performLookup.Return_AreExchangeRatesUpToDate_ToBool())
            {
                try
                {
                    Process exchangeRateProcess = Process.Start(exchangeRateExe);

                    if (!exchangeRateProcess.WaitForExit(timeoutMS))
                    {
                        exchangeRateProcess.Kill();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(ex.Message);
                    logger.Error(ex.StackTrace);
                }
            }
        }
    }
}
