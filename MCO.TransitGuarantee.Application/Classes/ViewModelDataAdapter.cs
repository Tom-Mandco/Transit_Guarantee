namespace MCO.TransitGuarantee.Application.Classes
{
    using System;
    using System.Collections.Generic;
    using Interfaces;
    using Models;
    using Data.Models;
    using Service.Interfaces;

    public class ViewModelDataAdapter : IViewModelDataAdapter
    {
        private readonly ILog logger;
        private readonly ICalculationHandler calculationHandler;
        private readonly IDateTimeConcatenator dtConcatenator;

        public ViewModelDataAdapter(ILog logger, ICalculationHandler calculationHandler, IDateTimeConcatenator dtConcatenator)
        {
            this.logger = logger;
            this.calculationHandler = calculationHandler;
            this.dtConcatenator = dtConcatenator;
        }

        public Invoice_Detail Map_InvoiceDetailDataModel_ToViewModel(InvoiceDetail_DataModel _invoiceDetail)
        {
            Invoice_Detail result = new Invoice_Detail()
            {
                Order_No = _invoiceDetail.Order_No,
                Lot_No = _invoiceDetail.Lot_No,
                Vat_A_Value = _invoiceDetail.Vat_A_Inv_Value,
                Vat_B_Value = _invoiceDetail.Vat_B_Inv_Value,
                Vat_C_Value = _invoiceDetail.Vat_C_Inv_Value,
                Vat_D_Value = _invoiceDetail.Vat_D_Inv_Value,
                Commodity_Code = _invoiceDetail.Commodity_Code,
                Commodity_Duty_Pct = _invoiceDetail.Duty_Per_Cent_Pcent
            };

            return result;
        }

        public Invoice_Header Map_InvoiceHeaderDataModel_ToViewModel(InvoiceHeader_DataModel _invoiceHeader, List<Invoice_Detail> result_invoiceDetails)
        {
            Invoice_Header result = new Invoice_Header()
            {
                Supplier_Invoice_Number = _invoiceHeader.Supplier_Invoice_No,
                Invoice_Currency = _invoiceHeader.Invoice_Currency,
                Invoice_Details = result_invoiceDetails
            };

            return result;
        }

        public Consignment Map_ConsignmentDataModel_ToViewModel(Consignment_DataModel _consignment, List<Invoice_Header> result_invoiceHeaders)
        {
            DateTime dtCustomsBooked = dtConcatenator.Return_DateAndTimeStrings_ToDateTime(_consignment.Booked_In_Date, _consignment.Booked_In_Time);
            DateTime dtETAatPort = Convert.ToDateTime(_consignment.ETA_At_Port);

            bool isActiveConsigment = Return_isConsignmentActive_ToBool(result_invoiceHeaders);

            Consignment result = new Consignment()
            {
                Consignment_Number = _consignment.Consignment_Number,
                Inland_Depot = _consignment.Inland_Depot,
                Carrier_Code = _consignment.Carrier_Code,
                Transport_Company = _consignment.Ship_Nameetruck_plat,
                Customs_Booked = dtCustomsBooked,
                ETA_At_Port = dtETAatPort,
                Active_Consignment = isActiveConsigment,
                Invoice_Headers = result_invoiceHeaders
            };

            return result;
        }

        public bool Return_isConsignmentActive_ToBool(List<Invoice_Header> _invoiceHeaders)
        {
            bool result = false;

            foreach (Invoice_Header _header in _invoiceHeaders)
            {
                foreach (Invoice_Detail _detail in _header.Invoice_Details)
                {
                    if(_detail.Date_of_WRC == null)
                    {
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
