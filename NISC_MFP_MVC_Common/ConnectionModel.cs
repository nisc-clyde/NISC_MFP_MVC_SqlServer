using Newtonsoft.Json;
using System.ComponentModel;

namespace NISC_MFP_MVC_Common
{
    public class ConnectionModel
    {
        [DisplayName("主機名稱")]
        [JsonProperty(PropertyName ="data source")]
        public string data_source { get; set; }

        [DisplayName("資料庫名稱")]
        [JsonProperty(PropertyName = "initial catalog")]
        public string initial_catalog { get; set; }

        [JsonProperty(PropertyName = "integrated security")]
        public bool integrated_security { get; set; }

        [DisplayName("資料庫帳號")]
        [JsonProperty(PropertyName = "user id")]
        public string user_id { get; set; }

        [DisplayName("資料庫密碼")]
        [JsonProperty(PropertyName = "password")]
        public string password { get; set; }

    }
}