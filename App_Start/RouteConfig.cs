using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
                name: "AdminRename",
                url: "Admin/{controller}/{action}/{id}",
                defaults: new { controller = "Print", action = "Print", id = UrlParameter.Optional },
                namespaces: new string[] { "NISC_MFP_MVC.Areas.Admin.Controllers" }
                );

            routes.MapRoute(
                name: "UserRoute",
                url: "User/{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "List", id = UrlParameter.Optional },
                namespaces: new string[] { "NISC_MFP_MVC.Controllers" }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "User", id = UrlParameter.Optional },
                namespaces: new string[] { "NISC_MFP_MVC.Controllers" }
            );
        }
    }
}
