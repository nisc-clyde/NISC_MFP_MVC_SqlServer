﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Common.Logger
{
    public class CardLogHandler : AbstractLogHandler
    {
        public override object LogHandle(string type, string operate, object data)
        {
            LogRequest logRequest = data as LogRequest;
            LogResponse logResponse = new LogResponse();
            if (type == "Card")
            {
                switch (operate)
                {
                    case "Add":
                        logResponse.Operation = "新增事務機";
                        logResponse.Message = $"(Id={logRequest.NewId}, Name={logRequest.NewContent})";
                        return logResponse;
                    case "Edit":
                        logResponse.Operation = "修改事務機";
                        logResponse.Message = $"(原)：(Id={logRequest.OldId}, Name={logRequest.OldContent}) \n" +
                            $"(新)：(Id={logRequest.NewId}, Name={logRequest.NewContent})";
                        return logResponse;
                    case "Delete":
                        logResponse.Operation = "刪除事務機";
                        logResponse.Message = $"(Id={logRequest.OldId}, Name={logRequest.OldContent})";
                        return logResponse;
                    case "AddManager":
                        logResponse.Operation = "儲值";
                        logResponse.Message = $"(原)：(Id={logRequest.OldId}, Name={logRequest.OldContent}) \n" +
                            $"(新)：(Id={logRequest.NewId}, Name={logRequest.NewContent})";
                        return logResponse;
                    case "EditManager":
                        logResponse.Operation = "重設免費點數";
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
