
using NISC_MFP_MVC.ViewModels.OutputReport;
using NISC_MFP_MVC_Service.DTOs.Info.OutputReport;
using NISC_MFP_MVC_Service.DTOs.Info.Print;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IOutputReportService
    {
        IEnumerable<UserInfo> GetAllUserByDepartmentId(string departmentId);
        IQueryable<PrintInfo> GetRecord(OutputReportRequestInfo outputReportRequestInfo);
        List<OutputReportUsageInfo> GetUsage(OutputReportRequestInfo outputReportRequestInfo);
    }
}
