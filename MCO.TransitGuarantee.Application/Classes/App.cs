﻿namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Models;
    using System;
    using System.Collections.Generic;
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
            try
            {
                totalTimeElapsed = new Stopwatch();
                sectionTimeElapsed = new Stopwatch();

                logger.Info("Transit Guarantee Started");

                totalTimeElapsed.Start();

                #region Run Exchange rate handler
                sectionTimeElapsed.Start();

                exchangeRateHandler.EnsureExchangeRatesAreCurrent();

                sectionTimeElapsed.Stop();
                logger.Info("Område tid: {0} | exchangeRates ", sectionTimeElapsed.Elapsed);
                sectionTimeElapsed.Reset();
                #endregion

                #region Return Consignment Data
                sectionTimeElapsed.Start();

                IEnumerable<Consignment> consignmentData = dataHandler.Return_AllActiveConsignments_ToViewModel();
                consignmentData = consignmentData.OrderBy(x => x.Consignment_Delivery_Status)
                                                 .ThenByDescending(x => x.ETA_At_Port)
                                                 .ThenBy(x => x.Consignment_Number);

                sectionTimeElapsed.Stop();
                logger.Info("Område tid: {0} | dataHandler ", sectionTimeElapsed.Elapsed);
                sectionTimeElapsed.Reset();
                #endregion

                #region Write to File
                
                sectionTimeElapsed.Start();
                fileWriter.Write_AllData_ToFile(consignmentData);
                sectionTimeElapsed.Stop();
                logger.Info("Område tid: {0} | fileWriter (txt)", sectionTimeElapsed.Elapsed);
                sectionTimeElapsed.Reset();
                
                #endregion

                #region Write to CSV
                sectionTimeElapsed.Start();
                fileWriter.Write_AllData_ToCsv(consignmentData);
                sectionTimeElapsed.Stop();
                logger.Info("Område tid: {0} | fileWriter (csv)", sectionTimeElapsed.Elapsed);
                sectionTimeElapsed.Reset();
                #endregion

                #region Write to Json
                sectionTimeElapsed.Start();
                fileWriter.Write_AllData_ToJson(consignmentData);
                sectionTimeElapsed.Stop();
                logger.Info("Område tid: {0} | fileWriter (jsn)", sectionTimeElapsed.Elapsed);
                sectionTimeElapsed.Reset();
                #endregion

                totalTimeElapsed.Stop();

                logger.Info("Tid taget:  {0}", totalTimeElapsed.Elapsed);
                logger.Info("Dette program er færdig.");

                logger.Info("Transit Guarantee Ended");
            }
            catch(Exception ex)
            {
                logger.Error(ex.Message);
                logger.Error(ex.StackTrace);
            }
        }
    }
}
