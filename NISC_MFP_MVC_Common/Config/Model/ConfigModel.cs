using Newtonsoft.Json;

namespace NISC_MFP_MVC_Common.Config.Model
{
    public class ConfigModel
    {
        [JsonProperty(PropertyName = "connection_string")]
        public ConnectionStringModel ConnectionString { get; set; }
        public string print_job_alive { get; set; }
    }
}
