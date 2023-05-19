using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_Print",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Print", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}