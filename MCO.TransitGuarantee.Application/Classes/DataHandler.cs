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
    using System.Linq;

    public class DataHandler : IDataHandler
    {
        private readonly IPerformLookup performLookup;
        private readonly IViewModelDataAdapter viewModelAdapter;
        private readonly ICalculationHandler calculationHandler;

        private static string InlandDepotList;
        private static string[] DutyFreeCountryCodeList;

        public DataHandler(IPerformLookup performLookup, IViewModelDataAdapter viewModelAdapter, ICalculationHandler calculationHandler)
        {
            this.performLookup = performLookup;
            this.viewModelAdapter = viewModelAdapter;
            this.calculationHandler = calculationHandler;

            InlandDepotList = ConfigurationManager.AppSettings["InlandDepotList"];
            DutyFreeCountryCodeList = ConfigurationManager.AppSettings["DutyFreeCountryCodeList"].Split(';');
        }

        public Dictionary<string, decimal> Return_ConsignmentTotals_ToDictionary(Consignment _consignment)
        {
            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            string consigmentTotalVATKey = ConfigurationManager.AppSettings["ConsTotalVATKey"];
            string consigmentTotalDutyKey = ConfigurationManager.AppSettings["ConsTotalDutyKey"];
            string consigmentTotalInTransitKey = ConfigurationManager.AppSettings["ConsTotalInTransitKey"];

            decimal VATRate1 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate1"]);
            decimal VATRate2 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate2"]);
            decimal VATRate3 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate3"]);
            decimal VATRate4 = Convert.ToDecimal(ConfigurationManager.AppSettings["VATRate4"]);

            decimal _invoiceTotalVAT = 0;
            decimal _invoiceTotalDuty = 0;
            decimal _invoiceTotalInTransit = 0;

            decimal detailTotalVAT;
            decimal detailTotalDuty;

            decimal vat_A;
            decimal vat_B;
            decimal vat_C;
            decimal vat_D;

            foreach (Invoice_Header _header in _consignment.Invoice_Headers)
            {
                foreach (Invoice_Detail _detail in _header.Invoice_Details)
                {
                    detailTotalVAT = 0;
                    detailTotalDuty = 0;

                    vat_A = calculationHandler.Return_LocalizedVAT_ToDecimal(_detail.Vat_A_Value, _header.Exchange_Rate);
                    vat_A = calculationHandler.Return_VATWithSupplierDiscount_ToDecimal(vat_A, _detail.Supplier_Discount_Pct);

                    vat_B = calculationHandler.Return_LocalizedVAT_ToDecimal(_detail.Vat_B_Value, _header.Exchange_Rate);
                    vat_B = calculationHandler.Return_VATWithSupplierDiscount_ToDecimal(vat_B, _detail.Supplier_Discount_Pct);

                    vat_C = calculationHandler.Return_LocalizedVAT_ToDecimal(_detail.Vat_C_Value, _header.Exchange_Rate);
                    vat_C = calculationHandler.Return_VATWithSupplierDiscount_ToDecimal(vat_C, _detail.Supplier_Discount_Pct);

                    vat_D = calculationHandler.Return_LocalizedVAT_ToDecimal(_detail.Vat_D_Value, _header.Exchange_Rate);
                    vat_D = calculationHandler.Return_VATWithSupplierDiscount_ToDecimal(vat_D, _detail.Supplier_Discount_Pct);

                    if (!DutyFreeCountryCodeList.Contains(_detail.Country_Code) && _detail.Commodity_Duty_Pct > 0)
                    {
                        detailTotalDuty += calculationHandler.Return_DutyDue_ToDecimal(vat_A, vat_B, vat_C, vat_D, _detail.Commodity_Duty_Pct);
                    }

                    detailTotalVAT += ((vat_A + detailTotalDuty) * VATRate1);
                    detailTotalVAT += (vat_B * VATRate2);
                    detailTotalVAT += (vat_C * VATRate3);
                    detailTotalVAT += (vat_D * VATRate4);

                    if (_consignment.Order_In_Transit)
                    {
                        _invoiceTotalInTransit += (detailTotalDuty + detailTotalVAT);
                    }

                    _invoiceTotalVAT += detailTotalVAT;
                    _invoiceTotalDuty += detailTotalDuty;
                }
            }

            result.Add(consigmentTotalVATKey, _invoiceTotalVAT);
            result.Add(consigmentTotalDutyKey, _invoiceTotalDuty);
            result.Add(consigmentTotalInTransitKey, _invoiceTotalInTransit);

            return result;
        }

        public bool Return_AreExchangeRatesUpToDate_ToBool()
        {
            return performLookup.Return_AreExchangeRatesUpToDate_ToBool();
        }

        public IEnumerable<Consignment> Return_AllActiveConsignments_ToViewModel()
        {
            List<Consignment> result = Return_Consignments_ToList();

            return result;
        }

        private List<Consignment> Return_Consignments_ToList()
        {
            var result = new List<Consignment>();

            IEnumerable<Consignment_DataModel> _consignmentDataModel = performLookup.Return_AllActiveConsignments_ToDataModel(InlandDepotList);

            foreach (Consignment_DataModel _consignment in _consignmentDataModel)
            {
                List<Invoice_Header> result_invoiceHeaders = Return_InvoiceHeaders_ToList(_consignment);

                Consignment _newConsignment = viewModelAdapter.Map_ConsignmentDataModel_ToViewModel(_consignment, result_invoiceHeaders);

                result.Add(_newConsignment);
            }

            return result;
        }

        private List<Invoice_Header> Return_InvoiceHeaders_ToList(Consignment_DataModel _consignment)
        {
            var result = new List<Invoice_Header>();

            IEnumerable<InvoiceHeader_DataModel> _invoiceHdrDataModel = performLookup.Return_ConsignmentInvoiceHeaders_ToDataModel(_consignment.Consignment_Number);

            if (_invoiceHdrDataModel != null)
            {
                foreach (InvoiceHeader_DataModel _invoiceHeader in _invoiceHdrDataModel)
                {
                    List<Invoice_Detail> result_invoiceDetails = Return_InvoiceDetails_ToList(_invoiceHeader);

                    Invoice_Header _newHeader = viewModelAdapter.Map_InvoiceHeaderDataModel_ToViewModel(_invoiceHeader, result_invoiceDetails);

                    result.Add(_newHeader);
                }
            }

            return result;
        }

        private List<Invoice_Detail> Return_InvoiceDetails_ToList(InvoiceHeader_DataModel _invoiceHeader)
        {
            var result = new List<Invoice_Detail>();

            IEnumerable<InvoiceDetail_DataModel> _invoiceDtlDataModel = performLookup.Return_ConsignmentInvoiceDetails_ToDataModel(_invoiceHeader.Consignment_Number, _invoiceHeader.Supplier_Invoice_No);

            foreach (InvoiceDetail_DataModel _invoiceDetail in _invoiceDtlDataModel)
            {
                Invoice_Detail _newDetail = viewModelAdapter.Map_InvoiceDetailDataModel_ToViewModel(_invoiceDetail);

                result.Add(_newDetail);
            }

            return result;
        }

    }
}
