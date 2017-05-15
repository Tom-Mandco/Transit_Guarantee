namespace MCO.TransitGuarantee.Data.Interfaces
{
    using System.Collections.Generic;
    using Models;

    public interface IRepository
    {
        IEnumerable<Consignment_DataModel> Fetch_AllActiveConsignments(string inlandDepotList);
        IEnumerable<InvoiceHeader_DataModel> Fetch_ConsignmentInvoiceHeaders(int consignment_Number);
        IEnumerable<InvoiceDetail_DataModel> Fetch_ConsignmentInvoiceDetails(int consignment_Number, string supplier_Invoice_Number);
    }
}
