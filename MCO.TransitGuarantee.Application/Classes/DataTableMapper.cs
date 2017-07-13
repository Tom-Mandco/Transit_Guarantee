namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Models;
    using Service.Interfaces;
    using System.Data;
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    class DataTableMapper : IDataTableMapper
    {
        private readonly IDataHandler dataHandler;
        private readonly IDataTableFactory dataTableFactory;

        public DataTableMapper(IDataHandler dataHandler, IDataTableFactory dataTableFactory)
        {
            this.dataHandler = dataHandler;
            this.dataTableFactory = dataTableFactory;
        }

        public DataTable Return_ConsignmentData_ToDataTable(IEnumerable<Consignment> consignmentData)
        {
            string consigmentTotalValueKey = ConfigurationManager.AppSettings["ConsTotalValueKey"];
            string consigmentTotalVATKey = ConfigurationManager.AppSettings["ConsTotalVATKey"];
            string consigmentTotalDutyKey = ConfigurationManager.AppSettings["ConsTotalDutyKey"];
            string consigmentTotalInTransitKey = ConfigurationManager.AppSettings["ConsTotalInTransitKey"];

            decimal transitGuaranteeRemaining = 0;
            decimal fullConsignmentValue = 0;

            decimal.TryParse(ConfigurationManager.AppSettings["TransitGuaranteeValue"], out transitGuaranteeRemaining);

            List<string> output_Breakdown = new List<string>();
            List<string> output_Concise = new List<string>();

            DataTable result = dataTableFactory.Return_EmptyConsignmentDTWithHeaders_ToDataTable();

            foreach (Consignment _consignment in consignmentData)
            {
                Dictionary<string, decimal> _ConsignmentTotals = dataHandler.Return_ConsignmentTotals_ToDictionary(_consignment);

                transitGuaranteeRemaining -= _ConsignmentTotals[consigmentTotalInTransitKey];
                fullConsignmentValue = _ConsignmentTotals[consigmentTotalVATKey] + _ConsignmentTotals[consigmentTotalDutyKey];

                result.Rows.Add(_consignment.Consignment_Number,
                                _consignment.Return_DeliveryStatus_ToString(),
                                Math.Round(_ConsignmentTotals[consigmentTotalDutyKey], 2),
                                Math.Round(_ConsignmentTotals[consigmentTotalVATKey], 2),
                                Math.Round(_ConsignmentTotals[consigmentTotalValueKey], 2),
                                Math.Round(fullConsignmentValue, 2),
                                Math.Round(_ConsignmentTotals[consigmentTotalInTransitKey], 2),
                                Math.Round(transitGuaranteeRemaining, 2)
                                );
            }

            return result;
        }
    }
}
