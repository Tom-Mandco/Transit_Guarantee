namespace MCO.TransitGuarantee.Application.Models
{
    using System;

    public class Invoice_Detail
    {
        public int Order_No { get; set; }
        public int Lot_No { get; set; }

        public string Commodity_Code { get; set; }
        public string Country_Code { get; set; }
        public string Country_Name { get; set; }
        public string Customs_Entry_No { get; set; }

        public decimal Vat_A_Value { get; set; }
        public decimal Vat_B_Value { get; set; }
        public decimal Vat_C_Value { get; set; }
        public decimal Vat_D_Value { get; set; }

        public decimal Commodity_Duty_Pct { get; set; }
        public decimal Supplier_Discount_Pct { get; set; }

        public bool orderInTransit { get; set; }

        public DateTime Date_of_WRC { get; set; }
        public DateTime ETA_At_Port { get; set; }
        public DateTime Date_of_Customs_Entry { get; set; }
    }
}