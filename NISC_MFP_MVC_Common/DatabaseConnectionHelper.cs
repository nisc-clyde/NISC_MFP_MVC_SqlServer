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
        //Server.MapPath()之根目錄為起始專案之目錄，回上層兩次後指到Common Library的conneciton_string.json
        private static readonly string PATH = Path.GetDirectoryName(HttpContext.Current.Server.MapPath("..")) + @"\NISC_MFP_MVC_Common\connection_string.json";

        private DatabaseConnectionHelper()
        {
        }

        /// <summary>
        /// For SQL Server Authentication
        /// </summary>
        /// <param name="data_source">主機名稱</param>
        /// <param name="initial_catalog">資料庫名稱</param>
        /// <param name="integrated_security">是否Windows驗證</param>
        /// <param name="user_id">資料庫帳號</param>
        /// <param name="password">資料庫密碼</param>
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

        /// <summary>
        /// 從connection_string.json轉換成SQL Connection String
        /// </summary>
        /// <returns>SQL Connection String</returns>
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

        /// <summary>
        /// ConnectionModel轉換成connection string後儲存在connectionString此static string
        /// </summary>
        /// <param name="connectionModel">欲儲存之ConnectionModel</param>
        /// <returns></returns>
        public static string ConvertModel2StringAndSave(ConnectionModel connectionModel)
        {
            if (connectionModel != null)
            {
                connectionString = "";
                //Reflection Object and convert to format-> 「${object json property name}=${object property value by name};」
                SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
                {
                    DataSource = connectionModel.data_source??"",
                    InitialCatalog = connectionModel.initial_catalog??"",
                    IntegratedSecurity = connectionModel.integrated_security,
                    UserID = connectionModel.user_id??"",
                    Password = connectionModel.password??"",
                };
                connectionString = sqlConnectionStringBuilder.ToString();

                return connectionString;
            }
            return null;
        }
    }
}