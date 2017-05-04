﻿namespace MCO.TemplateConsoleConnectionApp.Data.Classes
{
    using Interfaces;
    using MandCo.Data;

    public class OracleRepository : OracleBase, IRepository
    {
        public OracleRepository(string connectionString)
    : base(connectionString)
        {
        }
    }
}
