namespace MCO.TransitGuarantee.Application.Models
{
    using System;
    using System.Collections.Generic;

    public class Consignment
    {
        public int Consignment_Number { get; set; }
        public int Consignment_Delivery_Status { get; set; }

        public string Inland_Depot { get; set; }
        public string Carrier_Code { get; set; }
        public string Transport_Company { get; set; }

        public DateTime Customs_Booked { get; set; }
        public DateTime Booked_In_Date { get; set; }
        public DateTime ETA_At_Port { get; set; }

        public IEnumerable<Invoice_Header> Invoice_Headers { get; set; }

        public string Return_DeliveryStatus_ToString()
        {
            string result = "";

            switch (Consignment_Delivery_Status)
            {
                case 0:
                    result = "Forecast";
                    break;
                case 1:
                    result = "ETA Exceeded";
                    break;
                case 2:
                    result = "Active";
                    break;
                case 3:
                    result = "Delivered";
                    break;
            }

            return result;
        }
    }
}
