using System.Collections.Generic;
using MCO.TransitGuarantee.Application.Models;

namespace MCO.TransitGuarantee.Application.Interfaces
{
    public interface IFileWriter
    {
        void Write_AllData_ToFile(IEnumerable<Consignment> consignmentData);
        void Write_AllData_ToCsv(IEnumerable<Consignment> consignmentData);
    }
}
