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

            routes.MapRoute(
                name: "AdminRename",
                url: "admin/{action}/{id}",
                defaults: new { controller = "Admin", action = "Print", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "UserRoute",
                url: "user/{action}/{id}",
                defaults: new { controller = "User", action = "List", id = UrlParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "User", id = UrlParameter.Optional }
            );
        }
    }
}
