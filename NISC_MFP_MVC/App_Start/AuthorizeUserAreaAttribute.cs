using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NISC_MFP_MVC.App_Start
{
    public class AuthorizeUserAreaAttribute : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var userName = HttpContext.Current.User.Identity.Name;
            if (userName != null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { area = "", controller = "Login", action = "User" }));
            }
            else
            {
                base.HandleUnauthorizedRequest(filterContext);
            }

        }
    }
}