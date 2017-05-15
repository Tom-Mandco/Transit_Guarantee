namespace MCO.TransitGuarantee.Application.Models
{
    using System.Collections.Generic;

    public class Invoice_Header
    {
        public string Supplier_Invoice_Number { get; set; }
        public string Invoice_Currency { get; set; }

        public IEnumerable<Invoice_Detail> Invoice_Details { get; set; }
    }
}

