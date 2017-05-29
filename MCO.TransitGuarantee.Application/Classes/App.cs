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
        private static Stopwatch stopwatch;

        private readonly ILog logger;
        private readonly IDataHandler dataHandler;

        public App(ILog logger, IDataHandler dataHandler)
        {
            this.logger = logger;
            this.dataHandler = dataHandler;
        }

        public void Run()
        {
            logger.Info("Transit Guarantee Started");

            stopwatch = new Stopwatch();

            stopwatch.Start();

            IEnumerable<Consignment> consignmentData = dataHandler.Return_AllActiveConsignments_ToViewModel();

            consignmentData = consignmentData.OrderBy(x => x.Consignment_Delivery_Status);

            string consigmentTotalValueKey = ConfigurationManager.AppSettings["ConsTotalValueKey"];
            string consigmentTotalVATKey = ConfigurationManager.AppSettings["ConsTotalVATKey"];
            string consigmentTotalDutyKey = ConfigurationManager.AppSettings["ConsTotalDutyKey"];
            string consigmentTotalInTransitKey = ConfigurationManager.AppSettings["ConsTotalInTransitKey"];

            double transitGuaranteeRemaining = 0;

            double.TryParse(ConfigurationManager.AppSettings["TransitGuaranteeValue"], out transitGuaranteeRemaining);

            List<string> output_Breakdown = new List<string>();
            List<string> output_Concise = new List<string>();

            foreach (Consignment _consignment in consignmentData)
            {
                Dictionary<string, double> _ConsignmentTotals = dataHandler.Return_ConsignmentTotals_ToDictionary(_consignment);

                transitGuaranteeRemaining -= _ConsignmentTotals[consigmentTotalInTransitKey];

                output_Breakdown.Add(string.Format("Consignment: {0} | Carrier Code: {1} | Total Value: {2} | Duty: {3} | VAT: {4} | Active Consignment Value: {5}",
                                   _consignment.Consignment_Number,
                                   _consignment.Carrier_Code,
                                   _ConsignmentTotals[consigmentTotalValueKey],
                                   _ConsignmentTotals[consigmentTotalVATKey],
                                   _ConsignmentTotals[consigmentTotalDutyKey],
                                   _ConsignmentTotals[consigmentTotalInTransitKey]
                                   ));

                foreach(Invoice_Header _header in _consignment.Invoice_Headers)
                {
                    output_Breakdown.Add(string.Format(" - Supplier Invoice Number {0} | Invoice Currency: {1} ",
                                            _header.Supplier_Invoice_Number,
                                            _header.Invoice_Currency
                                            ));

                    foreach(Invoice_Detail _detail in _header.Invoice_Details)
                    {
                        output_Breakdown.Add(string.Format(" - - Order No: {0} | Lot No: {1} | Commodity Code: {6} | Duty Pct: {7} | VAT Invoice Values: {2} / {3} / {4} / {5}",
                                           _detail.Order_No,
                                           _detail.Lot_No,
                                           _detail.Vat_A_Value,
                                           _detail.Vat_B_Value,
                                           _detail.Vat_C_Value,
                                           _detail.Vat_D_Value,
                                           _detail.Commodity_Code,
                                           _detail.Commodity_Duty_Pct
                                           ));
                    }
                }


                output_Concise.Add(string.Format("[Status: {6}] [Active Transit Value: {0}] Consignment No: {1} | Total Value: {2} | Duty : {3} | VAT: {4} | Transit Guarantee Remaining: {5}",
                                    _ConsignmentTotals[consigmentTotalInTransitKey],
                                    _consignment.Consignment_Number,
                                    _ConsignmentTotals[consigmentTotalValueKey],
                                    _ConsignmentTotals[consigmentTotalVATKey],
                                    _ConsignmentTotals[consigmentTotalDutyKey],
                                    transitGuaranteeRemaining,
                                    _consignment.Return_DeliveryStatus_ToString()));
            }

            using (StreamWriter writer = new StreamWriter(@"F:\dev\Scripts and Snippets\SH_TransitGuarantee\Output\TransitGuaranteeOutput_Breakdown.txt"))
            {
                foreach (string line in output_Breakdown)
                {
                    writer.WriteLine(line);
                }
                writer.Close();
                writer.Dispose();
            }

            using (StreamWriter writer = new StreamWriter(@"F:\dev\Scripts and Snippets\SH_TransitGuarantee\Output\TransitGuaranteeOutput_Concise.txt"))
            {
                foreach (string line in output_Concise)
                {
                    writer.WriteLine(line);
                }
                writer.Close();
                writer.Dispose();
            }

            stopwatch.Stop();

            Console.WriteLine("Tid taget: {0}", stopwatch.Elapsed);
            Console.WriteLine("Dette program er færdig.");
            Console.ReadLine();

            logger.Info("Transit Guarantee Ended");
        }
    }
}
