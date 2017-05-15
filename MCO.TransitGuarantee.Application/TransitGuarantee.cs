namespace MCO.TransitGuarantee.Application
{
    using Interfaces;

    class TransitGuarantee
    {
        static void Main(string[] args)
        {
            CompositionRoot.Wire(new ApplicationModule());

            var app = CompositionRoot.Resolve<IApp>();

            app.Run();
        }
    }
}
