namespace MCO.TransitGuarantee.Application.Models
{
    using System;
    using System.Collections.Generic;

    public class Consignment
    {
        public int Consignment_Number { get; set; }

        public string Inland_Depot { get; set; }
        public string Carrier_Code { get; set; }
        public string Transport_Company { get; set; }

        public bool Active_Consignment { get; set; }

        public DateTime Customs_Booked { get; set; }
        public DateTime ETA_At_Port { get; set; }

        public IEnumerable<Invoice_Header> Invoice_Headers { get; set; }
    }
}
