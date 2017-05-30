Select consignment_number,
       booked_in_date,
       booked_in_time,
       ship_nametruck_plat,
       carrier_code,
       ETA_at_port,
       Inland_Depot
  From mackays.shp_consignments s
Where  s.inland_depot in (@0)
And    s.creation_date > sysdate - 60
--And	   s.last_update_usr_id = 'tsmith'
Order by consignment_number asc