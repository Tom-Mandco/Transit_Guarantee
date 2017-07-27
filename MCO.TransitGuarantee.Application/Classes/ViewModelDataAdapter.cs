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

        public ViewModelDataAdapter(ILog logger, ICalculationHandler calculationHandler)
        {
            this.logger = logger;
            this.calculationHandler = calculationHandler;
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
                Country_Name = _invoiceDetail.Country_Name,
                Country_Code = _invoiceDetail.Country_Code,
                Commodity_Duty_Pct = _invoiceDetail.Duty_Per_Cent_Pcent,
                Supplier_Discount_Pct = _invoiceDetail.Discount_Pcent,
                Date_of_WRC = _invoiceDetail.Confirmed_Date,
                ETA_At_Port = _invoiceDetail.ETA_At_Port
            };

            return result;
        }

        public Invoice_Header Map_InvoiceHeaderDataModel_ToViewModel(InvoiceHeader_DataModel _invoiceHeader, List<Invoice_Detail> result_invoiceDetails)
        {
            Invoice_Header result = new Invoice_Header()
            {
                Supplier_Invoice_Number = _invoiceHeader.Supplier_Invoice_No,
                Invoice_Currency = _invoiceHeader.Invoice_Currency,
                Invoice_Details = result_invoiceDetails,
                Exchange_Rate = _invoiceHeader.Exchange_Rate
            };

            return result;
        }

        public Consignment Map_ConsignmentDataModel_ToViewModel(Consignment_DataModel _consignment, List<Invoice_Header> result_invoiceHeaders)
        {
            DateTime dtCustomsBooked = Convert.ToDateTime(_consignment.Customs_Entered);
            DateTime dtBookedInDate = Convert.ToDateTime(_consignment.Booked_In_Date);
            DateTime dtETAatPort = Convert.ToDateTime(_consignment.ETA_At_Port);

            int _deliveryStatus = calculationHandler.Return_ConsignmentDeliveryStatus_ToInt(_consignment);
            bool _orderInTransit = calculationHandler.Return_IsOrderInTransit_ToBool(_consignment);

            Consignment result = new Consignment()
            {
                Consignment_Number = _consignment.Consignment_Number,
                Inland_Depot = _consignment.Inland_Depot,
                Carrier_Code = _consignment.Carrier_Code,
                Transport_Company = _consignment.Ship_Nameetruck_plat,
                Customs_Booked = dtCustomsBooked,
                Booked_In_Date = dtBookedInDate,
                ETA_At_Port = dtETAatPort,
                Consignment_Delivery_Status = _deliveryStatus,
                Invoice_Headers = result_invoiceHeaders,
                Supplier_Name = _consignment.Supplier_Name,
                Order_In_Transit = _orderInTransit
            };

            return result;
        }

        private int Return_ConsignmentDeliveryStatus_ToInt(List<Invoice_Header> result_invoiceHeaders)
        {
            int result = 0;
            bool allDelivered = true;
            bool customsEntered = false;
            bool ETAExceeded = false;

            if (result_invoiceHeaders.Count > 0)
            {
                foreach (Invoice_Header _header in result_invoiceHeaders)
                {
                    foreach (Invoice_Detail _detail in _header.Invoice_Details)
                    {
                        if (_detail.Date_of_WRC == DateTime.MinValue)
                        {
                            allDelivered = false;
                        }

                        if (_detail.Date_of_Customs_Entry > DateTime.MinValue)
                        {
                            customsEntered = true;
                        }

                        else if (_detail.ETA_At_Port < DateTime.Now)
                        {
                            ETAExceeded = true;
                        }
                    }
                }

                result = Return_StatusCodeFromBooleans_ToInt(allDelivered, customsEntered, ETAExceeded);
            }

            return result;
        }

        private int Return_StatusCodeFromBooleans_ToInt(bool _allDelivered, bool _customsEntered, bool _eTAExceeded)
        {
            int result = 0;

            if (_allDelivered)
            {
                result = 3;
            }

            else if (_customsEntered)
            {
                result = 2;
            }

            else if (_eTAExceeded)
            {
                result = 1;
            }

            return result;
        }
    }
}
