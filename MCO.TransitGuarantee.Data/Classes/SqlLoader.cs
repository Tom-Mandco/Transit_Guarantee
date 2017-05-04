namespace MCO.TemplateConsoleConnectionApp.Data.Classes
{
    using System.IO;

    public static class SqlLoader
    {
        public static string GetSql(string fileName)
        {
            var sqlFolder = System.Configuration.ConfigurationManager.AppSettings["SqlDirectory"];
            var sqlFileLocation = string.Format("{0}{1}.sql", sqlFolder, fileName);
            var sql = File.ReadAllText(sqlFileLocation);
            return sql;
        }
    }
}
