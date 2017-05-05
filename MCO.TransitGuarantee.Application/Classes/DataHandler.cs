namespace MCO.TransitGuarantee.Application.Classes
{
    using Interfaces;
    using Domain.Interfaces;

    public class DataHandler : IDataHandler
    {
        private readonly IPerformLookup performLookup;

        public DataHandler(IPerformLookup performLookup)
        {
            this.performLookup = performLookup;
        }
    }
}
