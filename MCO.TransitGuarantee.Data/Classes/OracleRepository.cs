namespace MCO.TransitGuarantee.Data.Classes
{
    using System.Linq;
    using System.Collections.Generic;
    using Interfaces;
    using MCO.Data;
    using Models;

    public class OracleRepository : OracleBase, IRepository
    {
        public OracleRepository(string connectionString)
    : base(connectionString)
        {
        }

        public bool Check_ExchangeRatesAreCurrent()
        {
            using (new SharedConnection(dbConnection))
            {
                var result = dbConnection.Query<ExchangeRate>(SqlLoader.GetSql("Check_ExchangeRatesAreCurrent"));

                return result.Any() ? false : true;
            }
        }

        public IEnumerable<Consignment_DataModel> Fetch_AllActiveConsignments(string inlandDepotList)
        {
            using (new SharedConnection(dbConnection))
            {
                var result = dbConnection.Query<Consignment_DataModel>(SqlLoader.GetSql("Fetch_AllActiveConsignments"),
                                                                             inlandDepotList);

                return result.Any() ? result : null;
            }
        }

        public IEnumerable<InvoiceDetail_DataModel> Fetch_ConsignmentInvoiceDetails(int consignment_Number, string supplier_Invoice_Number)
        {
            using (new SharedConnection(dbConnection))
            {
                var result = dbConnection.Query<InvoiceDetail_DataModel>(SqlLoader.GetSql("Fetch_ConsignmentInvoiceDetails"),
                                                                             consignment_Number,
                                                                             supplier_Invoice_Number
                                                                             );

                return result.Any() ? result : null;
            }
        }

        public IEnumerable<InvoiceHeader_DataModel> Fetch_ConsignmentInvoiceHeaders(int consignment_Number)
        {
            using (new SharedConnection(dbConnection))
            {
                var result = dbConnection.Query<InvoiceHeader_DataModel>(SqlLoader.GetSql("Fetch_ConsignmentInvoiceHeader"),
                                                                             consignment_Number
                                                                             );

                return result.Any() ? result : null;
            }
        }
    }
}
