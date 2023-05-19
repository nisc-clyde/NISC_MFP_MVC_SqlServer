
using NISC_MFP_MVC.ViewModels.OutputReport;
using NISC_MFP_MVC_Service.DTOs.Info.OutputReport;
using NISC_MFP_MVC_Service.DTOs.Info.Print;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using System.Collections.Generic;
using System.Linq;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IOutputReportService
    {
        /// <summary>
        /// 取得部門中所有User
        /// </summary>
        /// <param name="departmentId">部門dept_id</param>
        /// <returns></returns>
        IEnumerable<UserInfo> GetAllUserByDepartmentId(string departmentId);

        /// <summary>
        /// 取得Filter後的紀錄
        /// </summary>
        /// <param name="outputReportRequestInfo">Filter</param>
        /// <returns></returns>
        IQueryable<PrintInfo> GetRecord(OutputReportRequestInfo outputReportRequestInfo);

        /// <summary>
        /// 取得Filter後紀錄且小計的結果
        /// </summary>
        /// <param name="outputReportRequestInfo">Filter</param>
        /// <returns></returns>
        List<OutputReportUsageInfo> GetUsage(OutputReportRequestInfo outputReportRequestInfo);
    }
}
