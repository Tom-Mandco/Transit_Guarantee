Select shpCons.consignment_number,
       invHeader.Supplier_Invoice_No,
       invHeader.Invoice_Currency,
       invHeader.Invoice_Value,
       invHeader.Commission_Value,
       invHeader.Commission_Currency
  From shp_consignments shpCons,
       shp_supplier_inv_hdr invHeader
Where  shpCons.Consignment_Number = invHeader.Consignment_No
And    shpCons.Consignment_Number = @0
Order By shpCons.Consignment_Number,
         invHeader.Supplier_Invoice_No asc