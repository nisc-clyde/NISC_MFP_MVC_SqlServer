using Newtonsoft.Json;

namespace NISC_MFP_MVC_Common.Config.Model
{
    public class ConnectionStringModel
    {
        [JsonProperty(PropertyName = "Data Source")]
        public string DataSource { get; set; }

        [JsonProperty(PropertyName = "Initial Catalog")]
        public string InitialCatalog { get; set; }

        [JsonProperty(PropertyName = "Integrated Security")]
        public string IntegratedSecurity { get; set; }

        [JsonProperty(PropertyName = "User ID")]
        public string UserID { get; set; }

        [JsonProperty(PropertyName = "Password")]
        public string Password { get; set; }

        [JsonProperty(PropertyName = "Connect Timeout")]
        public string ConnectTimeout { get; set; }

        public override string ToString()
        {
            return
                $"Data Source={this.DataSource};Initial Catalog={this.InitialCatalog};Integrated Security={this.IntegratedSecurity};" +
                $"User ID={this.UserID};Password={this.Password};Connect Timeout={this.ConnectTimeout};";
        }
    }
}
