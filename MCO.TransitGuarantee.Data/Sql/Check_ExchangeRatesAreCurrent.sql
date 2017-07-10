Select *
From   shp_cons_exch_rates exchRates
Where  (extract (month from exchRates.Effective_Date) = extract(month from sysdate)
And     extract (year from exchRates.Effective_Date) = extract (year from sysdate))