using Google.Protobuf.WellKnownTypes;
using Microsoft.Build.Evaluation;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace NISC_MFP_MVC
{
    public class NLogHelper
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetLogger("OperationLog");
        public NLogHelper(string operation, string data)
        {
            FormsAuthenticationTicket ticket = ((FormsIdentity)HttpContext.Current.User.Identity).Ticket;

            Logger.WithProperty("login_user_id", HttpContext.Current.User.Identity.Name)
                        .WithProperty("login_user_name", ticket.UserData.Split(',').Last())
                        .WithProperty("operation", operation).Info(data);
        }
    }
}
