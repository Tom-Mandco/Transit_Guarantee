using MCO.TransitGuarantee.Data.Models;

namespace MCO.TransitGuarantee.Service.Interfaces
{
    public interface ICalculationHandler
    {
        bool Return_IsOrderInTransit_ToBool(InvoiceDetail_DataModel _invoiceDetail);
    }
}
