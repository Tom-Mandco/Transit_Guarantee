namespace MCO.TransitGuarantee.Service.Interfaces
{
    using System.Data;

    public interface IDataTableFactory
    {
        DataTable Return_EmptyConsignmentDTWithHeaders_ToDataTable();
    }
}
