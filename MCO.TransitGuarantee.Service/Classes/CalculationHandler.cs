namespace MCO.TransitGuarantee.Service.Classes
{
    using System;
    using Interfaces;
    using Data.Models;

    public class CalculationHandler : ICalculationHandler
    {
        public bool Return_IsOrderInTransit_ToBool(Consignment_DataModel _consignment)
        {
            bool result = false;

            var _etaAtPort = DateTime.MinValue;
            var _bookedInDate = DateTime.MinValue;
            var _customsEnteredDate = DateTime.MinValue;

            DateTime.TryParse(_consignment.ETA_At_Port, out _etaAtPort);
            DateTime.TryParse(_consignment.Booked_In_Date, out _bookedInDate);
            DateTime.TryParse(_consignment.Customs_Entered, out _customsEnteredDate);
            
            if (_customsEnteredDate > DateTime.MinValue)
            {
                if(_bookedInDate == DateTime.MinValue || _bookedInDate > DateTime.Now.Date)
                {
                    result = true;
                }
            }

            return result;
        }

        public decimal Return_LocalizedVAT_ToDecimal(decimal vatValue, decimal exchangeRate)
        {
            decimal result = 0;

            result = vatValue * exchangeRate;

            return result;
        }

        public decimal Return_VATWithSupplierDiscount_ToDecimal(decimal vatValue, decimal supplierDiscountPct)
        {
            decimal result = 0;

            result = vatValue - (vatValue * (supplierDiscountPct / 100));

            return result;
        }

        public decimal Return_DutyDue_ToDecimal(decimal vat_A, decimal vat_B, decimal vat_C, decimal vat_D, decimal Duty_Pct)
        {
            decimal result = 0;

            decimal dutyPct = (Duty_Pct / 100);

            result += vat_A;
            result += vat_B;
            result += vat_C;
            result += vat_D;

            result *= dutyPct;

            return result;
        }

        public int Return_ConsignmentDeliveryStatus_ToInt(Consignment_DataModel _consignment)
        {
            int result = 0;

            bool allDelivered = false;
            bool customsEntered = false;
            bool ETAExceeded = false;

            var _etaAtPort = DateTime.MinValue;
            var _bookedInDate = DateTime.MinValue;
            var _customsEnteredDate = DateTime.MinValue;

            DateTime.TryParse(_consignment.ETA_At_Port, out _etaAtPort);
            DateTime.TryParse(_consignment.Booked_In_Date, out _bookedInDate);
            DateTime.TryParse(_consignment.Customs_Entered, out _customsEnteredDate);

            if(_bookedInDate > DateTime.MinValue && _bookedInDate <= DateTime.Now)
            {
                allDelivered = true;
            }
            else if(_customsEnteredDate > DateTime.MinValue)
            {
                customsEntered = true;
            }
            else if(_etaAtPort <= DateTime.Now.Date)
            {
                ETAExceeded = true;
            }

            result = Return_StatusCodeFromBooleans_ToInt(allDelivered, customsEntered, ETAExceeded);

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
