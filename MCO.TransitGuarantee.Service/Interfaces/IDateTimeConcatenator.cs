using System;

namespace MCO.TransitGuarantee.Service.Interfaces
{
    public interface IDateTimeConcatenator
    {
        DateTime Return_DateAndTimeStrings_ToDateTime(string booked_In_Date, string booked_In_Time);
    }
}
