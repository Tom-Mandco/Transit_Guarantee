using MCO.TransitGuarantee.Data.Models;

namespace MCO.TransitGuarantee.Service.Interfaces
{
    public interface ICalculationHandler
    {
        bool Return_IsOrderInTransit_ToBool(Consignment_DataModel _consignment);
        decimal Return_LocalizedVAT_ToDecimal(decimal vatValue, decimal exchangeRate);
        decimal Return_VATWithSupplierDiscount_ToDecimal(decimal vatValue, decimal supplierDiscountPct);
        decimal Return_DutyDue_ToDecimal(decimal vat_A, decimal vat_B, decimal vat_C, decimal vat_D, decimal commodity_Duty_Pct);
        int Return_ConsignmentDeliveryStatus_ToInt(Consignment_DataModel _consignment);
    }
}
