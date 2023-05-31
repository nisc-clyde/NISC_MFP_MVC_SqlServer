using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Web;
using System.Web.UI;
using static System.Net.Mime.MediaTypeNames;

namespace NISC_MFP_MVC_Common
{
    public class DatabaseConnectionHelper
    {
        private static string connectionString = null;
        private static readonly string PATH = Path.GetDirectoryName(Path.GetDirectoryName(HttpContext.Current.Server.MapPath("~"))) + @"\NISC_MFP_MVC_Common\connection_string.json";

        private DatabaseConnectionHelper()
        {
        }

        /// <summary>
        /// For SQL Server Authentication
        /// </summary>
        /// <param name="data_source">主機名稱</param>
        /// <param name="initial_catalog">資料庫名稱</param>
        /// <param name="user_id">資料庫帳號</param>
        /// <param name="password">資料庫密碼</param>
        /// <param name="path">連線字串儲存位置</param>
        public static void SetConnectionString(string data_source, string initial_catalog, bool integrated_security = true, string user_id = "", string password = "")
        {
            ConnectionModel connectionModel = new ConnectionModel()
            {
                data_source = data_source,
                initial_catalog = initial_catalog,
                integrated_security = integrated_security,
                user_id = user_id,
                password = password
            };
            string output = JsonConvert.SerializeObject(connectionModel, Formatting.Indented);

            // output will override original json object
            File.WriteAllText(PATH, output);

            //Update Singleton String Object
            ConvertModel2StringAndSave(connectionModel);
        }

        public static string GetConnectionStringFromFile()
        {
            if (connectionString != null)
            {
                return connectionString;
            }
            string readText = File.ReadAllText(PATH);
            ConnectionModel newConnectionModel = JsonConvert.DeserializeObject<ConnectionModel>(readText);

            return ConvertModel2StringAndSave(newConnectionModel);
        }

        public static string ConvertModel2StringAndSave(ConnectionModel connectionModel)
        {
            if (connectionModel != null)
            {
                connectionString = "";
                //Reflection Object and convert to format-> 「${object json property name}=${object property value by name};」
                foreach (PropertyInfo property in connectionModel.GetType().GetProperties())
                {
                    var jsonPropertyAttribute = property.GetCustomAttribute<JsonPropertyAttribute>();
                    connectionString += $"{jsonPropertyAttribute.PropertyName}={property.GetValue(connectionModel)};";
                }

                return connectionString;
            }
            return null;
        }
    }
}