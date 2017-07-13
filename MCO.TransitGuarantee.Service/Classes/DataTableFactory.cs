
namespace MCO.TransitGuarantee.Service.Classes
{
    using Interfaces;
    using System.Data;

    public class DataTableFactory : IDataTableFactory
    {
        public DataTable Return_EmptyConsignmentDTWithHeaders_ToDataTable()
        {
            DataTable result = new DataTable();

            result.Columns.Add("Consignment Number");
            result.Columns.Add("Consignment Status");
            result.Columns.Add("Duty Value");
            result.Columns.Add("VAT Value");
            result.Columns.Add("Total Value");
            result.Columns.Add("Total Transit Value");
            result.Columns.Add("Active Transit Value");
            result.Columns.Add("Transit Guarantee Remaining");

            return result;
        }
    }
}
