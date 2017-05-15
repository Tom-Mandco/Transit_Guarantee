namespace MCO.TransitGuarantee.Service.Interfaces
{
    using Data.Models;

    public interface ICalculationHandler
    {
        bool Return_isConsignmentActive_ToBool(Consignment_DataModel _consignment);
    }
}
