using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.User
{
    public class UserAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "User";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "User_User",
                "User/User/{id}",
                new { controller = "User", action = "List", id = UrlParameter.Optional }
            );
        }
    }
}