namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;

    public class App : IApp
    {
        private readonly ILog logger;
        private readonly IDataHandler dataHandler;

        public App(ILog logger, IDataHandler dataHandler)
        {
            this.logger = logger;
            this.dataHandler = dataHandler;
        }
    }
}
