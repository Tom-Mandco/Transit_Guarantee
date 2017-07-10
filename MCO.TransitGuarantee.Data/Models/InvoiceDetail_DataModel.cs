﻿using System;

namespace MCO.TransitGuarantee.Data.Models
{
    public class InvoiceDetail_DataModel
    {
        public int Order_No { get; set; }
        public int Lot_No { get; set; }

        public string Commodity_Code { get; set; }
        public string Country_Code { get; set; }
        public string Country_Name { get; set; }

        public decimal Vat_A_Inv_Value { get; set; }
        public decimal Vat_B_Inv_Value { get; set; }
        public decimal Vat_C_Inv_Value { get; set; }
        public decimal Vat_D_Inv_Value { get; set; }

        public decimal Discount_Pcent { get; set; }
        public decimal Duty_Per_Cent_Pcent { get; set; }

        public DateTime Confirmed_Date { get; set; }
        public DateTime ETA_At_Port { get; set; }
        public DateTime Customs_Entered { get; set; }
    }
}