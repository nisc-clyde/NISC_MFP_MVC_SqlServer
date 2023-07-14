using NISC_MFP_MVC_Common.Config.Model;

namespace NISC_MFP_MVC.ViewModels.Config
{
    public class ConfigViewModel
    {
        public AdminRegister adminRegister { get; set; }

        public ConnectionStringModel connectionModel { get; set; }

        public ServerAddressModel serverAddresModel { get; set; }
    }
}