using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Common
{
    public static class DatabaseConnection
    {
        static MySqlConnection databaseConnection = null;
        public static MySqlConnection getDatabaseConnection()
        {
            if (databaseConnection == null)
            {
                string connectionString = ConfigurationManager.ConnectionStrings["MFP_DBEntities"].ConnectionString;
                Debug.WriteLine(connectionString);
                databaseConnection = new MySqlConnection(connectionString);
            }
            return databaseConnection;
        }
    }
}
