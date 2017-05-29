namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Domain.Interfaces;
    using Models;
    using Data.Models;
    using Service.Interfaces;
    using System;
    using System.Configuration;
    using System.Collections.Generic;

    public class DataHandler : IDataHandler
    {
        private readonly IPerformLookup performLookup;
        private readonly IViewModelDataAdapter viewModelAdapter;
        private readonly ICalculationHandler calculationHandler;

        public DataHandler(IPerformLookup performLookup, IViewModelDataAdapter viewModelAdapter, ICalculationHandler calculationHandler)
        {
            this.performLookup = performLookup;
            this.viewModelAdapter = viewModelAdapter;
            this.calculationHandler = calculationHandler;
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

        public double Return_ConsignmentTotalValue_ToDouble(Consignment _consignment)
        {
            double result = 0;


            return result;
        }

        public Dictionary<string, double> Return_ConsignmentTotals_ToDictionary(Consignment _consignment)
        {
            Dictionary<string, double> result = new Dictionary<string, double>();

            string consigmentTotalValueKey = ConfigurationManager.AppSettings["ConsTotalValueKey"];
            string consigmentTotalVATKey = ConfigurationManager.AppSettings["ConsTotalVATKey"];
            string consigmentTotalDutyKey = ConfigurationManager.AppSettings["ConsTotalDutyKey"];
            string consigmentTotalInTransitKey = ConfigurationManager.AppSettings["ConsTotalInTransitKey"];

            double VATRate1 = Convert.ToDouble(ConfigurationManager.AppSettings["VATRate1"]);
            double VATRate2 = Convert.ToDouble(ConfigurationManager.AppSettings["VATRate2"]);
            double VATRate3 = Convert.ToDouble(ConfigurationManager.AppSettings["VATRate3"]);
            double VATRate4 = Convert.ToDouble(ConfigurationManager.AppSettings["VATRate4"]);

            double _invoiceTotalValue = 0;
            double _invoiceTotalVAT = 0;
            double _invoiceTotalDuty = 0;
            double _invoiceTotalInTransit = 0;

            double detailTotalValue;
            double detailTotalVAT;
            double detailTotalDuty;

            foreach (Invoice_Header _header in _consignment.Invoice_Headers)
            {
                foreach (Invoice_Detail _detail in _header.Invoice_Details)
                {
                    detailTotalValue = 0;
                    detailTotalVAT = 0;
                    detailTotalDuty = 0;

                    detailTotalValue += _detail.Vat_A_Value;
                    detailTotalValue += _detail.Vat_B_Value;
                    detailTotalValue += _detail.Vat_C_Value;
                    detailTotalValue += _detail.Vat_D_Value;

                    detailTotalVAT += (_detail.Vat_A_Value * VATRate1);
                    detailTotalVAT += (_detail.Vat_B_Value * VATRate2);
                    detailTotalVAT += (_detail.Vat_C_Value * VATRate3);
                    detailTotalVAT += (_detail.Vat_D_Value * VATRate4);

                    detailTotalDuty += (_detail.Vat_A_Value * (_detail.Commodity_Duty_Pct / 100));
                    detailTotalDuty += (_detail.Vat_B_Value * (_detail.Commodity_Duty_Pct / 100));
                    detailTotalDuty += (_detail.Vat_C_Value * (_detail.Commodity_Duty_Pct / 100));
                    detailTotalDuty += (_detail.Vat_D_Value * (_detail.Commodity_Duty_Pct / 100));

                    _invoiceTotalValue += detailTotalValue;
                    _invoiceTotalVAT += detailTotalVAT;
                    _invoiceTotalDuty += detailTotalDuty;

                    if (_detail.orderInTransit)
                    {
                        _invoiceTotalInTransit += (detailTotalDuty + detailTotalVAT);
                    }
                }
            }

            result.Add(consigmentTotalValueKey, _invoiceTotalValue);
            result.Add(consigmentTotalVATKey, _invoiceTotalVAT);
            result.Add(consigmentTotalDutyKey, _invoiceTotalDuty);
            result.Add(consigmentTotalInTransitKey, _invoiceTotalInTransit);

            return result;
        }
    }
}
