using NISC_MFP_MVC_Common.Logger;

namespace NISC_MFP_MVC_Common
{
    public class Facade
    {
        protected DepartmentLogHandler departmentLogHandler;
        protected UserLogHandler userLogHandler;

        public Facade()
        {
            departmentLogHandler = new DepartmentLogHandler();
            userLogHandler = new UserLogHandler();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">部門=Department,使用者=User</param>
        /// <param name="operation">新增=Add,修改=Edit,刪除=Delete</param>
        /// <param name="data"></param>
        /// <returns></returns>
        public object Operation(string type, string operation, object data)
        {
            object result = "";
            result = departmentLogHandler.LogHandle(type, operation, data);
            result = userLogHandler.LogHandle(type, operation, data);
            return result;
        }
    }
}
