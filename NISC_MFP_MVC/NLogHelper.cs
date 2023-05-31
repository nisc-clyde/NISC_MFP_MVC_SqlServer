using Google.Protobuf.WellKnownTypes;
using Microsoft.Build.Evaluation;
using NISC_MFP_MVC_Common;
using NLog;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.Services.Description;
using System.Windows.Markup;

namespace NISC_MFP_MVC
{
    public class NLogHelper
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("OperationLog");
        private static readonly Lazy<NLogHelper> lazy = new Lazy<NLogHelper>(() => new NLogHelper());

        private NLogHelper()
        {
            var databaseTarget = (DatabaseTarget)LogManager.Configuration.FindTargetByName("Db");
            databaseTarget.ConnectionString = DatabaseConnectionHelper.GetConnectionStringFromFile();
            LogManager.ReconfigExistingLoggers();
        }

        public static NLogHelper Instance { get { return lazy.Value; } }

        public void Logging(string operation, string data)
        {
            if (HttpContext.Current.User != null && HttpContext.Current.User.Identity is FormsIdentity)
            {
                FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;
                Logger.WithProperty("login_user_id", HttpContext.Current.User.Identity.Name)
                            .WithProperty("login_user_name", ticket.UserData.Split(',').Last())
                            .WithProperty("operation", operation).Info(data);
            }
            else
            {
                //登入時，HttpContext還未帶入驗證，所以id和name為空
                Logger.WithProperty("login_user_id", "")
                            .WithProperty("login_user_name", "")
                            .WithProperty("operation", operation).Info(data);
            }
        }
    }
}
