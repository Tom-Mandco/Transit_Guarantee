namespace MCO.TemplateConsoleConnectionApp.Application
{
    using Classes;
    using Interfaces;
    using Data.Classes;
    using Data.Interfaces;
    using Domain.Classes;
    using Domain.Interfaces;

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
            Bind(typeof(IDataHandler)).To(typeof(DataHandler));
            Bind(typeof(IPerformLookup)).To(typeof(PerformLookup));
            Bind(typeof(IRepository)).To(typeof(OracleRepository)).InSingletonScope().WithConstructorArgument("connectionString", connectionString);
        }
    }
}
