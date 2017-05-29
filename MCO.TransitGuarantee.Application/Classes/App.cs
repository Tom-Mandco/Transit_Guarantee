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
        private readonly IFileWriter dataWriter;
        private readonly IDataHandler dataHandler;

        public App(ILog logger, IFileWriter dataWriter, IDataHandler dataHandler)
        {
            this.logger = logger;
            this.dataWriter = dataWriter;
            this.dataHandler = dataHandler;
        }

        public void Run()
        {
            totalTimeElapsed = new Stopwatch();
            sectionTimeElapsed = new Stopwatch();

            logger.Info("Transit Guarantee Started");
            Console.WriteLine("Programmet startede");

            totalTimeElapsed.Start();
            sectionTimeElapsed.Start();

            IEnumerable<Consignment> consignmentData = dataHandler.Return_AllActiveConsignments_ToViewModel();
            consignmentData = consignmentData.OrderBy(x => x.Consignment_Delivery_Status);

            sectionTimeElapsed.Stop();
            Console.WriteLine("Område tid: {0} | dataHandelr ", sectionTimeElapsed.Elapsed);
            sectionTimeElapsed.Reset();


            sectionTimeElapsed.Start();
            
            dataWriter.Write_AllData_ToFile(consignmentData);

            sectionTimeElapsed.Stop();
            Console.WriteLine("Område tid: {0} | dataWriter (File)", sectionTimeElapsed.Elapsed);
            sectionTimeElapsed.Reset();

            totalTimeElapsed.Stop();

            Console.WriteLine("Tid taget:  {0}", totalTimeElapsed.Elapsed);
            Console.WriteLine("Dette program er færdig.");
            Console.ReadLine();

            logger.Info("Transit Guarantee Ended");
        }
    }
}
