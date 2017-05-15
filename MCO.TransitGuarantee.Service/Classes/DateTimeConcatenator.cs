namespace MCO.TransitGuarantee.Service.Classes
{
    using System;
    using Interfaces;

    public class DateTimeConcatenator : IDateTimeConcatenator
    {
        public DateTime Return_DateAndTimeStrings_ToDateTime(string booked_In_Date, string booked_In_Time)
        {
            DateTime result = new DateTime();

            result = Convert.ToDateTime(booked_In_Date);

            return result;
        }
    }
}
