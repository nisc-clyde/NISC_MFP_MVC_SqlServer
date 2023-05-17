using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace NISC_MFP_MVC_Common.Logger
{
    public abstract class AbstractLogHandler : ILogHandler
    {
        private ILogHandler _nextLogHandler;

        public ILogHandler setNext(ILogHandler logHandler)
        {
            this._nextLogHandler = logHandler;
            return _nextLogHandler;
        }

        public virtual object LogHandle(string type, string operate, object data)
        {
            if (this._nextLogHandler != null)
            {
                return this._nextLogHandler.LogHandle(type, operate, data);
            }
            else
            {
                return null;
            }
        }
    }
}
