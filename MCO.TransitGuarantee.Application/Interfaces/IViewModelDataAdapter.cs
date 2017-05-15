using System.Collections.Generic;
using MCO.TransitGuarantee.Application.Models;
using MCO.TransitGuarantee.Data.Models;

namespace MCO.TransitGuarantee.Application.Interfaces
{
    public interface IViewModelDataAdapter
    {
        Invoice_Detail Map_InvoiceDetailDataModel_ToViewModel(InvoiceDetail_DataModel _invoiceDetail);
        Invoice_Header Map_InvoiceHeaderDataModel_ToViewModel(InvoiceHeader_DataModel _invoiceHeader, List<Invoice_Detail> result_invoiceDetails);
        Consignment Map_ConsignmentDataModel_ToViewModel(Consignment_DataModel _consignment, List<Invoice_Header> result_invoiceHeaders);
    }
}
