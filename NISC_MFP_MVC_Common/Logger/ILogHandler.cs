using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace NISC_MFP_MVC_Common.Logger
{
    public interface ILogHandler
    {
        ILogHandler setNext(ILogHandler logHandler);
        object LogHandle(string type, string operate, object data);
    }
}
