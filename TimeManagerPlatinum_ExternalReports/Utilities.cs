using DevExpress.XtraEditors;

namespace TimeManagerPlatinum_ExternalReports
{
    public static class Utilities
    {
        public static string BuildConnectionString(string path)
        {
            // Set the ServerType to 1 for connect to the embedded server
            string sDbConnectionString =
                "User=SYSDBA;" +
                "Password=masterkey;" +
                "Database=" + @path + ";" +
                "DataSource=localhost;" +
                "Port=3050;" +
                "Dialect=3;" +
                "Charset=NONE;" +
                "Role=;" +
                "Connection lifetime=15;" +
                "Pooling=true;" +
                "Packet Size=8192;" +
                "ServerType=0";

            return sDbConnectionString;
        }

        public static string GetDbPath()
        {
            string dbPath = Properties.Settings.Default.DB_Path;
            if (dbPath == "")
            {
                XtraMessageBox.Show("Set the db path under File > Database Path");
            }
            return dbPath;
        }
    }
}