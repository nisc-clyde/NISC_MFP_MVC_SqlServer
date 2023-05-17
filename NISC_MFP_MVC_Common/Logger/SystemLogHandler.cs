using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Common.Logger
{
    public class SystemLogHandler : AbstractLogHandler
    {
        public override object LogHandle(string type, string operate, object data)
        {
            LogRequest logRequest = data as LogRequest;
            LogResponse logResponse = new LogResponse();
            if (type == "System")
            {
                switch (operate)
                {
                    case "Import":
                        logResponse.Operation = "人事資料匯入 - 新增";
                        logResponse.Message = $"(Id={logRequest.NewId}, Name={logRequest.NewContent})";
                        return logResponse;
                    case "Reset":
                        logResponse.Operation = "人事資料匯入 - 重置";
                        logResponse.Message = $"(Id={logRequest.NewId}, Name={logRequest.NewContent})";
                        return logResponse;
                    default:
                        logResponse.Operation = "未知操作";
                        logResponse.Message = $"(Id={logRequest.OldId}, Name={logRequest.OldContent})";
                        return logResponse;
                }
            }
            else
            {
                return base.LogHandle(type, operate, data);
            }
        }
    }
}
