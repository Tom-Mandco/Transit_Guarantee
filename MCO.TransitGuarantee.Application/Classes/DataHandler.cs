namespace MCO.TransitGuarantee.Application.Classes
{
    using Data.Models;
    using Domain.Interfaces;
    using Interfaces;
    using Models;
    using Service.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Configuration;

    public class DataHandler : IDataHandler
    {
        private readonly IPerformLookup performLookup;
        private readonly IViewModelDataAdapter viewModelAdapter;
        private readonly ICalculationHandler calculationHandler;

        private static string InlandDepotList;

        public DataHandler(IPerformLookup performLookup, IViewModelDataAdapter viewModelAdapter, ICalculationHandler calculationHandler)
        {
            this.performLookup = performLookup;
            this.viewModelAdapter = viewModelAdapter;
            this.calculationHandler = calculationHandler;

            InlandDepotList = ConfigurationManager.AppSettings["InlandDepotList"];
        }

        public IEnumerable<Consignment> Return_AllActiveConsignments_ToViewModel()
        {
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
                        IEnumerable<InvoiceDetail_DataModel> _invoiceDtlDataModel = performLookup.Return_ConsignmentInvoiceDetails_ToDataModel(_invoiceHeader.Consignment_Number, _invoiceHeader.Supplier_Invoice_No);
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

        public Dictionary<string, decimal> Return_ConsignmentTotals_ToDictionary(Consignment _consignment)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            string consigmentTotalValueKey = ConfigurationManager.AppSettings["ConsTotalValueKey"];
            string consigmentTotalVATKey = ConfigurationManager.AppSettings["ConsTotalVATKey"];
            string consigmentTotalDutyKey = ConfigurationManager.AppSettings["ConsTotalDutyKey"];
            string consigmentTotalInTransitKey = ConfigurationManager.AppSettings["ConsTotalInTransitKey"];

            decimal VATRate1 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate1"]);
            decimal VATRate2 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate2"]);
            decimal VATRate3 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate3"]);
            decimal VATRate4 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate4"]);

            decimal _invoiceTotalValue = 0;
            decimal _invoiceTotalVAT = 0;
            decimal _invoiceTotalDuty = 0;
            decimal _invoiceTotalInTransit = 0;

            decimal detailTotalValue;
            decimal detailTotalVAT;
            decimal detailTotalDuty;

            decimal vat_A_WithSupplierDiscount;
            decimal vat_B_WithSupplierDiscount;
            decimal vat_C_WithSupplierDiscount;
            decimal vat_D_WithSupplierDiscount;

            foreach (Invoice_Header _header in _consignment.Invoice_Headers)
            {
                foreach (Invoice_Detail _detail in _header.Invoice_Details)
                {
                    vat_A_WithSupplierDiscount = ((_detail.Vat_A_Value + ((_detail.Vat_A_Value / 100) * _detail.Supplier_Discount_Pct)) * _header.Exchange_Rate);
                    vat_B_WithSupplierDiscount = ((_detail.Vat_B_Value + ((_detail.Vat_B_Value / 100) * _detail.Supplier_Discount_Pct)) * _header.Exchange_Rate);
                    vat_C_WithSupplierDiscount = ((_detail.Vat_C_Value + ((_detail.Vat_C_Value / 100) * _detail.Supplier_Discount_Pct)) * _header.Exchange_Rate);
                    vat_D_WithSupplierDiscount = ((_detail.Vat_D_Value + ((_detail.Vat_D_Value / 100) * _detail.Supplier_Discount_Pct)) * _header.Exchange_Rate);

                    detailTotalValue = 0;
                    detailTotalVAT = 0;
                    detailTotalDuty = 0;

                    detailTotalValue += vat_A_WithSupplierDiscount;
                    detailTotalValue += vat_B_WithSupplierDiscount;
                    detailTotalValue += vat_C_WithSupplierDiscount;
                    detailTotalValue += vat_D_WithSupplierDiscount;

                    if (_detail.Country_Code != "TR" || _detail.Country_Code != "BD")
                    {
                        detailTotalVAT += (vat_A_WithSupplierDiscount * VATRate1);
                        detailTotalVAT += (vat_B_WithSupplierDiscount * VATRate2);
                        detailTotalVAT += (vat_C_WithSupplierDiscount * VATRate3);
                        detailTotalVAT += (vat_D_WithSupplierDiscount * VATRate4);
                    }

                    detailTotalDuty += (vat_A_WithSupplierDiscount * (_detail.Commodity_Duty_Pct / 100));
                    detailTotalDuty += (vat_B_WithSupplierDiscount * (_detail.Commodity_Duty_Pct / 100));
                    detailTotalDuty += (vat_C_WithSupplierDiscount * (_detail.Commodity_Duty_Pct / 100));
                    detailTotalDuty += (vat_D_WithSupplierDiscount * (_detail.Commodity_Duty_Pct / 100));

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

        public bool Return_AreExchangeRatesUpToDate_ToBool()
        {
            return performLookup.Return_AreExchangeRatesUpToDate_ToBool();
        }
    }
}
