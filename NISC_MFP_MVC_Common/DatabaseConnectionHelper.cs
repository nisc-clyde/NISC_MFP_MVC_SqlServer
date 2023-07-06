using Newtonsoft.Json;
using System.Data.SqlClient;
using System.IO;
using System.Web;

namespace NISC_MFP_MVC_Common
{
    public class DatabaseConnectionHelper
    {
        private static readonly DatabaseConnectionHelper instance = new DatabaseConnectionHelper();
        private static string connectionString = null;

        //Server.MapPath()之根目錄為起始專案之目錄，回上層兩次後指到Common Library的conneciton_string.json
        private static readonly string PATH = HttpContext.Current.Server.MapPath(@"~\bin\") + @"\connection_string.json";

        static DatabaseConnectionHelper()
        {

        }

        private DatabaseConnectionHelper()
        {
        }

        public static DatabaseConnectionHelper Instance
        {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// For SQL Server Authentication
        /// </summary>
        /// <param name="data_source">主機名稱</param>
        /// <param name="initial_catalog">資料庫名稱</param>
        /// <param name="integrated_security">是否Windows驗證</param>
        /// <param name="user_id">資料庫帳號</param>
        /// <param name="password">資料庫密碼</param>
        public void SetConnectionString(string data_source, string initial_catalog, bool integrated_security = true, string user_id = "", string password = "")
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = data_source,
                InitialCatalog = initial_catalog,
                IntegratedSecurity = integrated_security,
                UserID = user_id,
                Password = password,
                ConnectTimeout = 15,
            };

            string output = JsonConvert.SerializeObject(sqlConnectionStringBuilder, Formatting.Indented);

            // output will override original json object
            File.WriteAllText(PATH, output);

            //Update Singleton String Object
            Save(sqlConnectionStringBuilder.ToString());
        }

        /// <summary>
        /// 取得Connection String
        /// </summary>
        /// <returns>Connection String</returns>
        public string GetConnectionString()
        {
#if DEBUG
            connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=mywebni1_managerc;Integrated Security=True;";
#endif
            return connectionString;
        }

        /// <summary>
        /// 從connection_string.json轉換成SqlConnectionStringBuilder
        /// </summary>
        /// <returns>SQL Connection String</returns>
        public string GetConnectionStringFromFile()
        {
            if (connectionString != null)
            {
                return connectionString;
            }
            string readText = File.ReadAllText(PATH);
            SqlConnectionStringBuilder sqlConnectionStringBuilder = JsonConvert.DeserializeObject<SqlConnectionStringBuilder>(readText);

            Save(sqlConnectionStringBuilder.ToString());

            return GetConnectionString();
        }

        /// <summary>
        /// 儲存Connection String
        /// </summary>
        /// <param name="newConnectionString">欲儲存之Connection String</param>
        public void Save(string newConnectionString)
        {
            connectionString = newConnectionString;
        }

        public SqlConnectionStringBuilder ConvertString2Model(string connectionString)
        {
            if (connectionString != null)
            {
                try
                {
                    SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder(connectionString);
                    return sqlConnectionStringBuilder;
                }
                catch
                {
                    return new SqlConnectionStringBuilder();
                }
            }
            return new SqlConnectionStringBuilder();
        }
    }
}