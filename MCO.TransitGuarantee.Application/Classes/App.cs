namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.IO;

    public class App : IApp
    {
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

            IEnumerable<Consignment> consignmentData = dataHandler.Return_AllActiveConsignments_ToViewModel();

            List<string> output = new List<string>();

            foreach (Consignment _consignment in consignmentData)
            {
                output.Add(string.Format("Consignment: {0} | Carrier Code: {1}",
                                   _consignment.Consignment_Number,
                                   _consignment.Carrier_Code
                                   ));

                foreach(Invoice_Header _header in _consignment.Invoice_Headers)
                {
                    output.Add(string.Format(" - Supplier Invoice Number {0} | Invoice Currency: {1} | Invoice Total : {2}|",
                                            _header.Supplier_Invoice_Number,
                                            _header.Invoice_Currency
                                            ));

                    foreach(Invoice_Detail _detail in _header.Invoice_Details)
                    {
                        output.Add(string.Format(" - - Order No: {0} | Lot No: {1} | Commodity Code: {6} | Duty Pct: {7} | VAT INV Values: {2} / {3} / {4} / {5}",
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
            }

            using (StreamWriter writer = new StreamWriter(@"F:\TransitGuaranteeOutput.txt"))
            {
                foreach (string line in output)
                {
                    writer.WriteLine(line);
                }
            }

            Console.WriteLine("Dette program er færdig.");
            Console.ReadLine();

            logger.Info("Transit Guarantee Ended");
        }
    }
}
