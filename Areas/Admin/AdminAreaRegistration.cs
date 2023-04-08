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
                "Admin/Print/{id}",
                new { controller = "Print", action = "Print", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_Deposite",
                "Admin/Deposite/{id}",
                new { controller = "Deposite", action = "Deposite", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_Department",
                "Admin/Department/{id}",
                new { controller = "Department", action = "Department", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_User",
                "Admin/User/{id}",
                new { controller = "User", action = "User", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_CardReader",
                "Admin/CardReader/{id}",
                new { controller = "CardReader", action = "CardReader", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_Card",
                "Admin/Card/{id}",
                new { controller = "Card", action = "Card", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_Watermark",
                "Admin/Watermark/{id}",
                new { controller = "Watermark", action = "Watermark", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_History",
                "Admin/History/{id}",
                new { controller = "History", action = "History", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_System",
                "Admin/System/{id}",
                new { controller = "System", action = "System", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_OutputReport",
                "Admin/OutputReport/{id}",
                new { controller = "OutputReport", action = "OutputReport", id = UrlParameter.Optional }
            );

            context.MapRoute(
                "Admin_LogOut",
                "Admin/LogOut/{id}",
                new { controller = "LogOut", action = "LogOut", id = UrlParameter.Optional }
            );
        }
    }
}