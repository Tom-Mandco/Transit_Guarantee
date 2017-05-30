namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Models;
    using System.Data;
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    class DataTableFactory : IDataTableFactory
    {
        private readonly IDataHandler dataHandler;

        public DataTableFactory(IDataHandler dataHandler)
        {
            this.dataHandler = dataHandler;
        }

        public DataTable Return_ConsignmentData_ToDataTable(IEnumerable<Consignment> consignmentData)
        {
            string consigmentTotalValueKey = ConfigurationManager.AppSettings["ConsTotalValueKey"];
            string consigmentTotalVATKey = ConfigurationManager.AppSettings["ConsTotalVATKey"];
            string consigmentTotalDutyKey = ConfigurationManager.AppSettings["ConsTotalDutyKey"];
            string consigmentTotalInTransitKey = ConfigurationManager.AppSettings["ConsTotalInTransitKey"];

            double transitGuaranteeRemaining = 0;
            double fullConsignmentValue = 0;

            double.TryParse(ConfigurationManager.AppSettings["TransitGuaranteeValue"], out transitGuaranteeRemaining);

            DataTable result = Return_EmptyConsignmentDTWithHeaders_ToDataTable();

            List<string> output_Breakdown = new List<string>();
            List<string> output_Concise = new List<string>();

            foreach (Consignment _consignment in consignmentData)
            {
                Dictionary<string, double> _ConsignmentTotals = dataHandler.Return_ConsignmentTotals_ToDictionary(_consignment);

                transitGuaranteeRemaining -= _ConsignmentTotals[consigmentTotalInTransitKey];
                fullConsignmentValue = _ConsignmentTotals[consigmentTotalVATKey] + _ConsignmentTotals[consigmentTotalDutyKey];

                result.Rows.Add(_consignment.Consignment_Number,
                                _consignment.Return_DeliveryStatus_ToString(),
                                _ConsignmentTotals[consigmentTotalDutyKey],
                                _ConsignmentTotals[consigmentTotalVATKey],
                                _ConsignmentTotals[consigmentTotalValueKey],
                                fullConsignmentValue,
                                _ConsignmentTotals[consigmentTotalInTransitKey],
                                transitGuaranteeRemaining
                                );
            }

            return result;
        }

        private DataTable Return_EmptyConsignmentDTWithHeaders_ToDataTable()
        {
            DataTable result = new DataTable();

            result.Columns.Add("Consignment Number");
            result.Columns.Add("Consignment Status");
            result.Columns.Add("Duty Value");
            result.Columns.Add("VAT Value");
            result.Columns.Add("Total Value");
            result.Columns.Add("Total Transit Value");
            result.Columns.Add("Active Transit Value");
            result.Columns.Add("Transit Guarantee Remaining");

            return result;
        }
    }
}
