Select shpCons.consignment_number,
       invHeader.Supplier_Invoice_No,
       invHeader.Invoice_Currency,
       invHeader.Invoice_Value,
       invHeader.Commission_Value,
       invHeader.Commission_Currency,
       exchRates.Exchange_Rate
  From shp_consignments shpCons,
       shp_supplier_inv_hdr invHeader,
       shp_cons_exch_rates exchRates
Where  shpCons.Consignment_Number = invHeader.Consignment_No
And    shpCons.Consignment_Number = @0
And    (invHeader.Invoice_Currency = exchRates.To_Country_Code
And    (Extract(Month From sysdate) = Extract(Month From sysdate))
And     Extract (Year from sysdate) = Extract (Year from sysdate))
Order By shpCons.Consignment_Number,
         invHeader.Supplier_Invoice_No Asc