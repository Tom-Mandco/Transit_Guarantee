namespace MCO.TransitGuarantee.Service.Classes
{
    using System;
    using Interfaces;
    using Data.Models;

    public class CalculationHandler : ICalculationHandler
    {
        public bool Return_IsOrderInTransit_ToBool(InvoiceDetail_DataModel _invoiceDetail)
        {
            bool result = false;

            if(_invoiceDetail.Confirmed_Date == new DateTime())
            {
                if (_invoiceDetail.Customs_Entered > new DateTime())
                {
                    result = true;
                }
                else if(_invoiceDetail.ETA_At_Port < DateTime.Now)
                {
                    //Kept in for detailed status code 
                    result = true;
                }
            }

            return result;
        }
    }
}
