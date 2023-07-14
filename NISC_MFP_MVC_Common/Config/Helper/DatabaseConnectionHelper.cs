using System.IO;
using Newtonsoft.Json;
using NISC_MFP_MVC_Common.Config.Model;

namespace NISC_MFP_MVC_Common.Config.Helper
{
    public class DatabaseConnectionHelper : ConfigBase<ConnectionStringModel>
    {
        private static readonly DatabaseConnectionHelper instance = new DatabaseConnectionHelper();
        
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
        
        public override ConnectionStringModel Get()
        {
            ConnectionStringModel model = GetFile().ConnectionString;
#if DEBUG
            model = new ConnectionStringModel()
            {
                DataSource = ".\\SQLEXPRESS",
                InitialCatalog = "mywebni1_managerc",
                IntegratedSecurity = "True"
            };
            return model;
#endif

            if (model == null)
            {
                return null;
            }
            return model;
        }

        public override void Set(ConnectionStringModel entity)
        {
            //取出Json並序列化
            ConfigModel originalConfigModel = GetFile();

            //修改參數
            if (!string.IsNullOrWhiteSpace(entity.DataSource))
                originalConfigModel.ConnectionString.DataSource = entity.DataSource;
            if (!string.IsNullOrWhiteSpace(entity.InitialCatalog))
                originalConfigModel.ConnectionString.InitialCatalog = entity.InitialCatalog;
            if (!string.IsNullOrWhiteSpace(entity.IntegratedSecurity))
                originalConfigModel.ConnectionString.IntegratedSecurity = entity.IntegratedSecurity;
            if (!string.IsNullOrWhiteSpace(entity.UserID))
                originalConfigModel.ConnectionString.UserID = entity.UserID;
            if (!string.IsNullOrWhiteSpace(entity.Password))
                originalConfigModel.ConnectionString.Password = entity.Password;
            if (!string.IsNullOrWhiteSpace(entity.ConnectTimeout))
                originalConfigModel.ConnectionString.ConnectTimeout = entity.ConnectTimeout;

            //輸出參數
            string output = JsonConvert.SerializeObject(originalConfigModel, Formatting.Indented);
            
            //寫回參數
            File.WriteAllText(PATH, output);
        }
    }
}