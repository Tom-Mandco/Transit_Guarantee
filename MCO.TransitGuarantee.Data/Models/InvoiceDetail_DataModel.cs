using System;

namespace MCO.TransitGuarantee.Data.Models
{
    public class InvoiceDetail_DataModel
    {
        public int Order_No { get; set; }
        public int Lot_No { get; set; }

        public string Commodity_Code { get; set; }

        public double Vat_A_Inv_Value { get; set; }
        public double Vat_B_Inv_Value { get; set; }
        public double Vat_C_Inv_Value { get; set; }
        public double Vat_D_Inv_Value { get; set; }

        public double Duty_Per_Cent_Pcent { get; set; }
    }
}
