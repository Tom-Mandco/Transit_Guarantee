namespace MCO.TransitGuarantee.Domain.Classes
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Data.Models;
    using Data.Interfaces;

    public class PerformLookup : IPerformLookup
    {
        private readonly IRepository repository;

        public PerformLookup(IRepository repository)
        {
            this.repository = repository;
        }

        public IEnumerable<Consignment_DataModel> Return_AllActiveConsignments_ToDataModel(string inlandDepotList)
        {
            return repository.Fetch_AllActiveConsignments(inlandDepotList);
        }

        public IEnumerable<InvoiceHeader_DataModel> Return_ConsignmentInvoiceHeaders_ToDataModel(int consignment_Number)
        {
            return repository.Fetch_ConsignmentInvoiceHeaders(consignment_Number);
        }

        public IEnumerable<InvoiceDetail_DataModel> Return_ConsignmentInvoiceDetails_ToDataModel(int consignment_Number, string supplier_Invoice_Number)
        {
            return repository.Fetch_ConsignmentInvoiceDetails(consignment_Number, supplier_Invoice_Number);
        }

    }
}
