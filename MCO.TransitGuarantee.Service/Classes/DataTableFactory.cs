
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
            result.Columns.Add("Supplier Name");
            result.Columns.Add("Consignment Status");
            result.Columns.Add("ETA At Port");
            result.Columns.Add("Customs Entered");
            result.Columns.Add("Booked In Date");
            result.Columns.Add("Duty Value");
            result.Columns.Add("VAT Value");
            result.Columns.Add("Active Transit Value");
            result.Columns.Add("Transit Guarantee Remaining");

            return result;
        }
    }
}
