using NISC_MFP_MVC_Common;
using System.Data.SqlClient;

namespace NISC_MFP_MVC.ViewModels.Config
{
    public class ConfigViewModel
    {
        public AdminRegister adminRegister { get; set; }

        public SqlConnectionStringBuilder connectionModel { get; set; }
    }
}