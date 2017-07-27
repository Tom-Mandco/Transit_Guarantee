Select distinct shpCons.consignment_number,
       invHeader.Supplier_Invoice_No,
       countries.country_name,
       countries.country_code,
       invDetail.Order_No,
       invDetail.Lot_No,
       invDetail.Commodity_Code,
       shpComms.Duty_Per_Cent_Pcent,
       invDetail.Vat_a_Inv_Value,
       invDetail.Vat_b_Inv_Value,
       invDetail.Vat_c_Inv_Value,
       invDetail.Vat_d_Inv_Value,
       supp.discount_pcent,
       wrc.Confirmed_Date,
       shpCons.Eta_At_Port,
       shpCons.Customs_Entered
  From shp_consignments shpCons,
       shp_commodity_codes shpComms,
       shp_supplier_inv_hdr invHeader,
       shp_supplier_inv_dtl invDetail,
       ref_supplier supp,
       shp_countries countries,
       shp_ports port,
      (Select Min(wrcHeader.Confirmed_Date) "CONFIRMED_DATE",
              wrcDetail.Order_No
       From   war_wrc_header wrcHeader,
                  war_wrc_detail wrcDetail
       Where  wrcDetail.Wrc_Number = wrcHeader.Wrc_Number (+)
       Group By wrcDetail.Order_No) wrc
Where  shpCons.Consignment_Number = invHeader.Consignment_No
And    shpCons.Consignment_Number = invDetail.Consignment_Number
And    shpCons.Point_Of_Departure = port.ce_port_code
And    port.country_code = countries.country_code
And    invDetail.Commodity_Code = shpComms.Commodity_Code (+)
And    invDetail.Order_No = wrc.Order_No (+)
And    invHeader.Supplier_Account = supp.supplier_account
And    invHeader.Supplier_Invoice_No = invDetail.Supplier_Invoice_No
And    invHeader.Consignment_No = @0
And    invHeader.Supplier_Invoice_No = @1
Order By shpCons.Consignment_Number,
         invDetail.Order_No,
         invDetail.Lot_No,
         invdetail.commodity_code Asc