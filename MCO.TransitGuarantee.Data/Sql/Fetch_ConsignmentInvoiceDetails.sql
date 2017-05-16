Select shpCons.consignment_number,
       invHeader.Supplier_Invoice_No,
       invDetail.Order_No,
       invDetail.Lot_No,
       invDetail.Commodity_Code,
       shpComms.Duty_Per_Cent_Pcent,
       invDetail.Vat_a_Inv_Value,
       invDetail.Vat_b_Inv_Value,
       invDetail.Vat_c_Inv_Value,
       invDetail.Vat_d_Inv_Value,
       wrcHeader.Confirmed_Date
  From shp_consignments shpCons,
       shp_commodity_codes shpComms,
       shp_supplier_inv_hdr invHeader,
       shp_supplier_inv_dtl invDetail,
       war_wrc_header wrcHeader,
       war_wrc_detail wrcDetail
Where  shpCons.Consignment_Number = invHeader.Consignment_No
And    shpCons.Consignment_Number = invDetail.Consignment_Number
And    invDetail.Commodity_Code = shpComms.Commodity_Code (+)
And    invDetail.Order_No = wrcDetail.Order_No (+)
And    wrcDetail.Wrc_Number = wrcHeader.Wrc_Number (+)
And    invHeader.Supplier_Invoice_No = invDetail.Supplier_Invoice_No
And    invHeader.Consignment_No = @0
And    invHeader.Supplier_Invoice_No = @1
Order By shpCons.Consignment_Number,
         invDetail.Order_No,
         invDetail.Lot_No,
         invdetail.commodity_code Asc