using System.IO;
using Newtonsoft.Json;
using NISC_MFP_MVC_Common.Config.Model;
using System.Web;

namespace NISC_MFP_MVC_Common.Config.Helper
{
    public class ServerAddressHelper
    {
        private static readonly ServerAddressHelper instance = new ServerAddressHelper();
        protected static readonly string PATH = HttpContext.Current.Server.MapPath(@"~\bin\") + @"\server_address.json";

        static ServerAddressHelper()
        {

        }

        private ServerAddressHelper()
        {

        }

        public static ServerAddressHelper Instance
        {
            get
            {
                return instance;
            }
        }

        public ServerAddressModel Get()
        {
            string readText = File.ReadAllText(PATH);
            ServerAddressModel configBuilder = JsonConvert.DeserializeObject<ServerAddressModel>(readText);

            return configBuilder;
        }

        public void Set(ServerAddressModel model)
        {
            string readText = File.ReadAllText(PATH);
            ServerAddressModel configBuilder = JsonConvert.DeserializeObject<ServerAddressModel>(readText);

            configBuilder.ServerAddress = model.ServerAddress;

            string output = JsonConvert.SerializeObject(configBuilder, Formatting.Indented);

            File.WriteAllText(PATH, output);
        }


    }
}
