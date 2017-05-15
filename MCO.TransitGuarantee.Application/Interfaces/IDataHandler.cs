namespace MCO.TransitGuarantee.Application.Interfaces
{
    using Models;
    using System.Collections.Generic;

    public interface IDataHandler
    {
        IEnumerable<Consignment> Return_AllActiveConsignments_ToViewModel();
    }
}
