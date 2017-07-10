Insert Into Shp_Cons_Exch_Rates (FROM_CURRENCY, TO_CURRENCY, EFFECTIVE_DATE, EXCHANGE_RATE, LAST_UPDATE_USR_ID, LAST_UPDATE_DATE, FROM_COUNTRY_CODE, TO_COUNTRY_CODE)
Select :fromCur, :toCur, :effDate, :exchRate, :usr, :usrTime, :fromCountry, :toCountry From Dual
Where  Not Exists(Select * 
                  From Shp_Cons_Exch_Rates r 
                  Where r.from_currency = :fromCur
                  And   r.to_currency = :toCur
                  And   r.effective_date = :effDate)