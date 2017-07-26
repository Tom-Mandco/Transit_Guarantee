namespace MCO.TransitGuarantee.Service.Classes
{
    using System;
    using Interfaces;
    using Data.Models;

    public class CalculationHandler : ICalculationHandler
    {
        public bool Return_IsOrderInTransit_ToBool(InvoiceDetail_DataModel _invoiceDetail)
        {
            bool result = false;

            if (_invoiceDetail.Confirmed_Date == new DateTime())
            {
                if (_invoiceDetail.Customs_Entered > new DateTime())
                {
                    result = true;
                }
            }

            return result;
        }

        public decimal Return_LocalizedVAT_ToDecimal(decimal vatValue, decimal exchangeRate)
        {
            decimal result = 0;

            result = vatValue * exchangeRate;

            return result;
        }

        public decimal Return_VATWithSupplierDiscount_ToDecimal(decimal vatValue, decimal supplierDiscountPct)
        {
            decimal result = 0;

            result = vatValue - (vatValue * (supplierDiscountPct / 100));

            return result;
        }

        public decimal Return_DutyDue_ToDecimal(decimal vat_A, decimal vat_B, decimal vat_C, decimal vat_D, decimal Duty_Pct)
        {
            decimal result = 0;

            decimal dutyPct = (Duty_Pct / 100);

            result += vat_A;
            result += vat_B;
            result += vat_C;
            result += vat_D;

            result *= dutyPct;

            return result;
        }
    }
}
