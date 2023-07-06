using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Config
{
    public class ConfigAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Config";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Config_DatabaseConnection",
                "Config/{controller}/{action}/{id}",
                new { controller = "DatabaseConnection", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}