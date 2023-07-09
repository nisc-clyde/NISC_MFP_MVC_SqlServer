using System.IO;
using System.Web;
using Newtonsoft.Json;
using NISC_MFP_MVC_Common.Config.Model;

namespace NISC_MFP_MVC_Common.Config
{
    public abstract class ConfigBase<TEntity>
    where TEntity : class
    {
        protected static readonly string PATH = HttpContext.Current.Server.MapPath(@"~\bin\") + @"\config.json";

        public abstract TEntity Get();
        public abstract void Set(TEntity entity);

        protected ConfigModel GetFile()
        {
            string readText = File.ReadAllText(PATH);
            ConfigModel configBuilder = JsonConvert.DeserializeObject<ConfigModel>(readText);

            return configBuilder;
        }

    }
}
