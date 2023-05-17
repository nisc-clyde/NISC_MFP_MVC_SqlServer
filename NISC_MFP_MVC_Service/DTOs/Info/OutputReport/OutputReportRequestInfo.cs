using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC.ViewModels.OutputReport
{
    public class OutputReportRequestInfo
    {
        public string reportType { get; set; }
        public string reportColor { get; set; }
        public string deptId { get; set; }
        public string userId { get; set; }
        public string mfpIp { get; set; }
        public string date { get; set; }
    }
}
