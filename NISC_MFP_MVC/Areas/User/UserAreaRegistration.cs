using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName => "User";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "User_User",
                "User/{controller}/{action}/{id}",
                new { controller = "User", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}