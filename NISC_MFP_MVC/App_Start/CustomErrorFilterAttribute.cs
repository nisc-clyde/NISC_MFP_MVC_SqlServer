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
                logger.Error(filterContext.Exception.ToString(),"Exception End");
                filterContext.ExceptionHandled = true;
            }
        }
    }
}