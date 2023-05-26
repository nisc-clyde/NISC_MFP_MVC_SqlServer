using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace NISC_MFP_MVC.App_Start
{
    public class CustomErrorFilterAttribute : FilterAttribute, IExceptionFilter
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public void OnException(ExceptionContext filterContext)
        {
            if (!filterContext.ExceptionHandled)
            {
                var controllerName = filterContext.RouteData.Values["controller"];
                var actionName = filterContext.RouteData.Values["action"];
                logger.Error($"發生Controller：{controllerName}\n發生Action：{actionName}\n錯誤訊息：{ filterContext.Exception.ToString()}","Exception End");
                filterContext.ExceptionHandled = true;
            }
        }
    }
}