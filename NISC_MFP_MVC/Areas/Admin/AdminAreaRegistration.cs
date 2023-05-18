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

            //context.MapRoute(
            //    "Admin_Deposit",
            //    "Admin/Deposit/{id}",
            //    new { controller = "Deposit", action = "Deposit", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_Department",
            //    "Admin/Department/{id}",
            //    new { controller = "Department", action = "Department", id = UrlParameter.Optional }
            //);

            ////todo
            //context.MapRoute(
            //    "Admin_AddDepartment",
            //    "Admin/AddDepartment/{id}",
            //    new { controller = "Department", action = "AddDepartment", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_UpdateDepartment",
            //    "Admin/UpdateDepartment/{id}",
            //    new { controller = "Department", action = "UpdateDepartment", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_User",
            //    "Admin/User/{id}",
            //    new { controller = "User", action = "User", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_AddUser",
            //    "Admin/AddUser/{id}",
            //    new { controller = "User", action = "AddUser", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_SearchDepartment",
            //    "Admin/SearchDepartment/{id}",
            //    new { controller = "User", action = "SearchDepartment", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_CardReader",
            //    "Admin/CardReader/{id}",
            //    new { controller = "CardReader", action = "CardReader", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_AddCardReader",
            //    "Admin/AddCardReader/{id}",
            //    new { controller = "CardReader", action = "AddCardReader", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_Card",
            //    "Admin/Card/{id}",
            //    new { controller = "Card", action = "Card", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_AddCard",
            //    "Admin/AddCard/{id}",
            //    new { controller = "Card", action = "AddCard", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_ResetCardFreePoint",
            //    "Admin/ResetCardFreePoint/{id}",
            //    new { controller = "Card", action = "ResetCardFreePoint", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_Watermark",
            //    "Admin/Watermark/{id}",
            //    new { controller = "Watermark", action = "Watermark", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_AddWatermark",
            //    "Admin/AddWatermark/{id}",
            //    new { controller = "Watermark", action = "AddWatermark", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_History",
            //    "Admin/History/{id}",
            //    new { controller = "History", action = "History", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_System",
            //    "Admin/System/{id}",
            //    new { controller = "System", action = "System", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_OutputReport",
            //    "Admin/OutputReport/{id}",
            //    new { controller = "OutputReport", action = "OutputReport", id = UrlParameter.Optional }
            //);

            //context.MapRoute(
            //    "Admin_LogOut",
            //    "Admin/LogOut/{id}",
            //    new { controller = "LogOut", action = "LogOut", id = UrlParameter.Optional }
            //);
        }
    }
}