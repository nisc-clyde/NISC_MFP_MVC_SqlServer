using System.Collections.Generic;
using System.Web.Mvc;

namespace NISC_MFP_MVC.ViewModels.OutputReport
{
    public class OutputReportViewModel : AbstractViewModel
    {
        public OutputReportViewModel()
        {
            departmentNames = new List<SelectListItem>();
            multiFunctionPrints = new List<SelectListItem>();
        }

        public int reportType { get; set; }

        public int colorType { get; set; }

        public List<SelectListItem> departmentNames { get; set; }

        public List<SelectListItem> multiFunctionPrints { get; set; }

        public int duration { get; set; }
    }
}