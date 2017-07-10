namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Diagnostics;
    using System.Linq;

    public class App : IApp
    {
        private static Stopwatch totalTimeElapsed;
        private static Stopwatch sectionTimeElapsed;

        private readonly ILog logger;
        private readonly IFileWriter fileWriter;
        private readonly IDataHandler dataHandler;
        private readonly IExchangeRateHandler exchangeRateHandler;

        public App(ILog logger, IFileWriter dataWriter, IDataHandler dataHandler, IExchangeRateHandler exchangeRateHandler)
        {
            this.logger = logger;
            this.fileWriter = dataWriter;
            this.dataHandler = dataHandler;
            this.exchangeRateHandler = exchangeRateHandler;
        }

        public void Run()
        {
            totalTimeElapsed = new Stopwatch();
            sectionTimeElapsed = new Stopwatch();

            logger.Info("Transit Guarantee Started");
            Console.WriteLine("Programmet startede");

            totalTimeElapsed.Start();

            sectionTimeElapsed.Start();

            //if (dataHandler.Return_AreExchangeRatesUpToDate_ToBool())
                exchangeRateHandler.EnsureExchangeRatesAreCurrent();

            sectionTimeElapsed.Stop();
            Console.WriteLine("Område tid: {0} | exchangeRates ", sectionTimeElapsed.Elapsed);
            sectionTimeElapsed.Reset();


            sectionTimeElapsed.Start();

            IEnumerable<Consignment> consignmentData = dataHandler.Return_AllActiveConsignments_ToViewModel();
            consignmentData = consignmentData.OrderBy(x => x.Consignment_Delivery_Status);

            sectionTimeElapsed.Stop();
            Console.WriteLine("Område tid: {0} | dataHandler ", sectionTimeElapsed.Elapsed);
            sectionTimeElapsed.Reset();


            sectionTimeElapsed.Start();
            fileWriter.Write_AllData_ToFile(consignmentData);
            sectionTimeElapsed.Stop();
            Console.WriteLine("Område tid: {0} | fileWriter (txt)", sectionTimeElapsed.Elapsed);
            sectionTimeElapsed.Reset();


            sectionTimeElapsed.Start();
            fileWriter.Write_AllData_ToCsv(consignmentData);
            sectionTimeElapsed.Stop();
            Console.WriteLine("Område tid: {0} | fileWriter (csv)", sectionTimeElapsed.Elapsed);
            sectionTimeElapsed.Reset();


            sectionTimeElapsed.Start();
            fileWriter.Write_AllData_ToJson(consignmentData);
            sectionTimeElapsed.Stop();
            Console.WriteLine("Område tid: {0} | fileWriter (jsn)", sectionTimeElapsed.Elapsed);
            sectionTimeElapsed.Reset();


            totalTimeElapsed.Stop();

            Console.WriteLine("Tid taget:  {0}", totalTimeElapsed.Elapsed);
            Console.WriteLine("Dette program er færdig.");
            Console.ReadLine();

            logger.Info("Transit Guarantee Ended");
        }
    }
}
