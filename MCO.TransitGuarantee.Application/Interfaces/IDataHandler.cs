namespace MCO.TransitGuarantee.Application.Interfaces
{
    using Models;
    using System.Collections.Generic;

    public interface IDataHandler
    {
        IEnumerable<Consignment> Return_AllActiveConsignments_ToViewModel();
        Dictionary<string, decimal> Return_ConsignmentTotals_ToDictionary(Consignment _consignment);
        bool Return_AreExchangeRatesUpToDate_ToBool();
    }
}
