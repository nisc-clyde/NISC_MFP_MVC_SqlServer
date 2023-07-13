using System.Web.Mvc;
using System.Web.Routing;

namespace NISC_MFP_MVC
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "LoginRoute",
                url: "{controller}/{action}",
                defaults: new { controller = "Login", action = "User" }
                );

            routes.MapRoute(
                name: "AdminRoute",
                url: "Admin/{controller}/{action}",
                defaults: new { controller = "Print", action = "Index" },
                namespaces: new string[] { "NISC_MFP_MVC.Areas.Admin.Controllers" }
                );

            routes.MapRoute(
                name: "UserRoute",
                url: "User/{controller}/{action}",
                defaults: new { controller = "User", action = "Index" },
                namespaces: new string[] { "NISC_MFP_MVC.Areas.User.Controllers" }
                );

            routes.MapRoute(
                name: "ConfigRoute",
                url: "Config/{controller}/{action}",
                defaults: new { controller = "Dashboard", action = "Index" },
                namespaces: new string[] { "NISC_MFP_MVC.Areas.Config.Controllers" }
            );
        }
    }
}
