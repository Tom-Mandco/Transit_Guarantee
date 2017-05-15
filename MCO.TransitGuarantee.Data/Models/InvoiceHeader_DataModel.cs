namespace MCO.TransitGuarantee.Data.Models
{
    using System.Collections.Generic;

    public class InvoiceHeader_DataModel
    {
        public int Consignment_Number { get; set; }
        public string Invoice_Currency { get; set; }
        public string Supplier_Invoice_No { get; set; }

        public IEnumerable<InvoiceDetail_DataModel> Invoice_Details { get; set; }
    }
}
