namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Models;
    using System.Collections.Generic;
    using System.Configuration;
    using System;
    using System.IO;
    using System.Data;
    using ClosedXML.Excel;
    using ClosedXML;
    using Newtonsoft.Json;
    using System.Globalization;

    public class FileWriter : IFileWriter
    {
        private readonly IDataHandler dataHandler;
        private readonly IDataTableMapper dtFactory;
        private static string outputDirectory;
        private static string outputFileName;

        public FileWriter(IDataHandler dataHandler, IDataTableMapper dtFactory)
        {
            this.dataHandler = dataHandler;
            this.dtFactory = dtFactory;

            outputDirectory = ConfigurationManager.AppSettings["OutputFileLocation"];

            outputFileName = string.Format("TransitGuarantee_{1}",
                                        outputDirectory,
                                        DateTime.Now.ToString("yyyyMMdd"));

            EnsureDirectoryExists(outputDirectory);
        }

        public void Write_AllData_ToFile(IEnumerable<Consignment> consignmentData)
        {
            string consigmentTotalVATKey = ConfigurationManager.AppSettings["ConsTotalVATKey"];
            string consigmentTotalDutyKey = ConfigurationManager.AppSettings["ConsTotalDutyKey"];
            string consigmentTotalInTransitKey = ConfigurationManager.AppSettings["ConsTotalInTransitKey"];

            decimal transitGuaranteeRemaining = 0;
            decimal fullConsignmentValue = 0;

            decimal.TryParse(ConfigurationManager.AppSettings["TransitGuaranteeValue"], out transitGuaranteeRemaining);

            List<string> output_Breakdown = new List<string>();
            List<string> output_Concise = new List<string>();

            foreach (Consignment _consignment in consignmentData)
            {
                Dictionary<string, decimal> _ConsignmentTotals = dataHandler.Return_ConsignmentTotals_ToDictionary(_consignment);

                transitGuaranteeRemaining -= _ConsignmentTotals[consigmentTotalInTransitKey];
                fullConsignmentValue = _ConsignmentTotals[consigmentTotalVATKey] + _ConsignmentTotals[consigmentTotalDutyKey];

                output_Breakdown.Add(string.Format("Consignment: {0} | Carrier Code: {1} | Duty: {2} | VAT: {3} | Active Consignment Value: {4}",
                                   _consignment.Consignment_Number,
                                   _consignment.Carrier_Code,
                                   _ConsignmentTotals[consigmentTotalVATKey].ToString("C2", CultureInfo.GetCultureInfo("en-GB")),
                                   _ConsignmentTotals[consigmentTotalDutyKey].ToString("C2", CultureInfo.GetCultureInfo("en-GB")),
                                   _ConsignmentTotals[consigmentTotalInTransitKey].ToString("C2", CultureInfo.GetCultureInfo("en-GB"))
                                   ));

                foreach (Invoice_Header _header in _consignment.Invoice_Headers)
                {
                    output_Breakdown.Add(string.Format(" - Supplier Invoice Number {0} | Invoice Currency: {1} ",
                                            _header.Supplier_Invoice_Number,
                                            _header.Invoice_Currency
                                            ));

                    foreach (Invoice_Detail _detail in _header.Invoice_Details)
                    {
                        output_Breakdown.Add(string.Format(" - - Order No: {0} | Lot No: {1} | Commodity Code: {6} | Duty Pct: {7} | VAT Invoice Values: {2} / {3} / {4} / {5}",
                                           _detail.Order_No,
                                           _detail.Lot_No,
                                           _detail.Vat_A_Value.ToString("C2", CultureInfo.GetCultureInfo("en-GB")),
                                           _detail.Vat_B_Value.ToString("C2", CultureInfo.GetCultureInfo("en-GB")),
                                           _detail.Vat_C_Value.ToString("C2", CultureInfo.GetCultureInfo("en-GB")),
                                           _detail.Vat_D_Value.ToString("C2", CultureInfo.GetCultureInfo("en-GB")),
                                           _detail.Commodity_Code,
                                           _detail.Commodity_Duty_Pct.ToString("N1")
                                           ));
                    }
                }


                output_Concise.Add(string.Format("[Status: {5}] [Active Transit Value: {0} / {6}] Consignment No: {1} | Duty : {2} | VAT: {3} | Transit Guarantee Remaining: {4}",
                                    _ConsignmentTotals[consigmentTotalInTransitKey],
                                    _consignment.Consignment_Number,
                                    _ConsignmentTotals[consigmentTotalVATKey],
                                    _ConsignmentTotals[consigmentTotalDutyKey],
                                    transitGuaranteeRemaining,
                                    _consignment.Return_DeliveryStatus_ToString(),
                                    fullConsignmentValue));
            }

            using (StreamWriter writer = new StreamWriter(outputDirectory + outputFileName + "_Breakdown.txt"))
            {
                foreach (string line in output_Breakdown)
                {
                    writer.WriteLine(line);
                }
                writer.Close();
                writer.Dispose();
            }

            using (StreamWriter writer = new StreamWriter(outputDirectory + outputFileName + ".txt"))
            {
                foreach (string line in output_Concise)
                {
                    writer.WriteLine(line);
                }
                writer.Close();
                writer.Dispose();
            }
        }

        public void Write_AllData_ToCsv(IEnumerable<Consignment> consignmentData)
        {
            XLWorkbook newWorkbook = new XLWorkbook();
            DataTable dtConsignmentData = dtFactory.Return_ConsignmentData_ToDataTable(consignmentData);

            string workbookName = string.Format("{0}",
                        DateTime.Now.ToString("dd-MMM-yyyy"));

            string workbookFilePath = string.Format("{0}{1}.{2}",
                                                    outputDirectory,
                                                    outputFileName,
                                                    ConfigurationManager.AppSettings["ExcelOutputFileExtension"]);

            var workSheet = newWorkbook.Worksheets.Add(dtConsignmentData, workbookName);

            workSheet.Cells("A2:A999").DataType = XLCellValues.Number;

            workSheet.Cells("C2:E999").DataType = XLCellValues.DateTime;
            workSheet.Cells("C2:E999").Style.DateFormat.Format = "dd/mm/yyyy";

            workSheet.Cells("F2:K999").DataType = XLCellValues.Number;
            workSheet.Cells("F2:K999").Style.NumberFormat.Format = "£ #,##0.00";

            workSheet.Columns().AdjustToContents();

            newWorkbook.SaveAs(workbookFilePath);
        }

        public void Write_AllData_ToJson(IEnumerable<Consignment> consignmentData)
        {
            var _jsonSerializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore
            };

            string fullFilePath = string.Format("{0}{1}.Json",
                                                outputDirectory,
                                                outputFileName);

            string json = JsonConvert.SerializeObject(consignmentData, Formatting.Indented, _jsonSerializerSettings);

            File.WriteAllText(fullFilePath, json);
        }

        private void EnsureDirectoryExists(string directoryPath)
        {
            if(!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
        }
    }
}
