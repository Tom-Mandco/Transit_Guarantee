namespace MCO.TransitGuarantee.Application.Interfaces
{
    using System.Collections.Generic;
    using System.Data;
    using Models;

    public interface IDataTableMapper
    {
        DataTable Return_ConsignmentData_ToDataTable(IEnumerable<Consignment> consignmentData);
    }
}
