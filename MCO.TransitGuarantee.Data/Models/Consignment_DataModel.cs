namespace MCO.TransitGuarantee.Data.Models
{
    using System;
    using System.Collections.Generic;

    public class Consignment_DataModel
    {
        public string Inland_Depot { get; set; }
        public string Ship_Nameetruck_plat { get; set; }
        public string Carrier_Code { get; set; }
        public string Supplier_Name { get; set; }

        public int Consignment_Number { get; set; }

        public string Customs_Entered { get; set; }
        public string Booked_In_Date { get; set; }
        public string Booked_In_Time { get; set; }
        public string ETA_At_Port { get; set; }

        public IEnumerable<InvoiceHeader_DataModel> Invoice_Headers { get; set; }
    }
}
