using MySql.Data.MySqlClient;

namespace NISC_MFP_MVC_Common
{
    public static class DatabaseConnection
    {
        static MySqlConnection databaseConnection = null;
        static string DatabaseServer = "";
        static string DatabaseName = "";
        static string DatabaseUser = "";
        static string DatabasePassword = "";

        public static MySqlConnection getDatabaseConnection()
        {
            //if (databaseConnection == null)
            //{
            //    string connectionString = ConfigurationManager.ConnectionStrings["MFP_DBEntities"].ConnectionString;
            //    Debug.WriteLine(connectionString);
            //    databaseConnection = new MySqlConnection(connectionString);
            //}
            //return databaseConnection;

            return databaseConnection;
        }

        public static void setDatabaseConnection(string server, string name, string user, string password)
        {
            //@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;"
            databaseConnection = new MySqlConnection($@"Server={server};Database={name};Uid={user};Pwd={password}");
        }
    }
}
