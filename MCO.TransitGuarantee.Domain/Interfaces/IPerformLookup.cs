namespace MCO.TransitGuarantee.Domain.Interfaces
{
    using Data.Models;
    using System.Collections.Generic;

    public interface IPerformLookup
    {
        IEnumerable<Consignment_DataModel> Return_AllActiveConsignments_ToDataModel(string inlandDepotList);
        IEnumerable<InvoiceHeader_DataModel> Return_ConsignmentInvoiceHeaders_ToDataModel(int consignment_Number);
        IEnumerable<InvoiceDetail_DataModel> Return_ConsignmentInvoiceDetails_ToDataModel(int consignment_Number, string supplier_Invoice_Number);
        bool Return_AreExchangeRatesUpToDate_ToBool();
    }
}
