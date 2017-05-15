namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Domain.Interfaces;
    using Models;
    using Data.Models;
    using System;
    using System.Configuration;
    using System.Collections.Generic;

    public class DataHandler : IDataHandler
    {
        private readonly IPerformLookup performLookup;
        private readonly IViewModelDataAdapter viewModelAdapter;

        public DataHandler(IPerformLookup performLookup, IViewModelDataAdapter viewModelAdapter)
        {
            this.performLookup = performLookup;
            this.viewModelAdapter = viewModelAdapter;
        }

        public IEnumerable<Consignment> Return_AllActiveConsignments_ToViewModel()
        {
            string InlandDepotList = ConfigurationManager.AppSettings["InlandDepotList"];

            IEnumerable<Consignment_DataModel> _consignmentDataModel = performLookup.Return_AllActiveConsignments_ToDataModel(InlandDepotList);

            List<Consignment> result = new List<Consignment>();



            foreach (Consignment_DataModel _consignment in _consignmentDataModel)
            {
                IEnumerable<InvoiceHeader_DataModel> _invoiceHdrDataModel = performLookup.Return_ConsignmentInvoiceHeaders_ToDataModel(_consignment.Consignment_Number);

                List<Invoice_Header> result_invoiceHeaders = new List<Invoice_Header>();

                
                if (_invoiceHdrDataModel != null)
                {
                    foreach (InvoiceHeader_DataModel _invoiceHeader in _invoiceHdrDataModel)
                    {
                        IEnumerable<InvoiceDetail_DataModel> _invoiceDtlDataModel = performLookup.Return_ConsignmentInvoiceDetails_ToDataModel(_invoiceHeader.Consignment_Number,
                                                                                                                                               _invoiceHeader.Supplier_Invoice_No);

                        List<Invoice_Detail> result_invoiceDetails = new List<Invoice_Detail>();

                        foreach (InvoiceDetail_DataModel _invoiceDetail in _invoiceDtlDataModel)
                        {
                            Invoice_Detail _newDetail = viewModelAdapter.Map_InvoiceDetailDataModel_ToViewModel(_invoiceDetail);

                            result_invoiceDetails.Add(_newDetail);
                        }

                        Invoice_Header _newHeader = viewModelAdapter.Map_InvoiceHeaderDataModel_ToViewModel(_invoiceHeader, result_invoiceDetails);

                        result_invoiceHeaders.Add(_newHeader);
                    }
                }

                Consignment _newConsignment = viewModelAdapter.Map_ConsignmentDataModel_ToViewModel(_consignment, result_invoiceHeaders);

                result.Add(_newConsignment);
            }
            return result;
        }
    }
}
