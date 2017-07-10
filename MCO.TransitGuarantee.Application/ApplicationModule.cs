namespace MCO.TransitGuarantee.Application
{
    using Classes;
    using Interfaces;
    using Data.Classes;
    using Data.Interfaces;
    using Domain.Classes;
    using Domain.Interfaces;
    using Service.Classes;
    using Service.Interfaces;

    using NLog;
    using Ninject.Modules;
    using System.Configuration;

    class ApplicationModule : NinjectModule
    {
        public override void Load()
        {
            string connectionString = "";
            try
            {
                connectionString = ConfigurationManager.AppSettings["OracleConnection"];
            }
            catch
            {

            }

            Bind<ILog>().ToMethod(x =>
            {
                var scope = x.Request.ParentRequest.Service.FullName;
                var log = (ILog)LogManager.GetLogger(scope, typeof(Log));
                return log;
            });

            Bind(typeof(IApp)).To(typeof(App));
            Bind(typeof(IFileWriter)).To(typeof(FileWriter));
            Bind(typeof(IDataHandler)).To(typeof(DataHandler));
            Bind(typeof(IPerformLookup)).To(typeof(PerformLookup));
            Bind(typeof(IDataTableFactory)).To(typeof(DataTableFactory));
            Bind(typeof(ICalculationHandler)).To(typeof(CalculationHandler));
            Bind(typeof(IExchangeRateHandler)).To(typeof(ExchangeRateHandler));
            Bind(typeof(IDateTimeConcatenator)).To(typeof(DateTimeConcatenator));
            Bind(typeof(IViewModelDataAdapter)).To(typeof(ViewModelDataAdapter));
            Bind(typeof(IRepository)).To(typeof(OracleRepository)).InSingletonScope().WithConstructorArgument("connectionString", connectionString);
        }
    }
}
