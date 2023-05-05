using System.Collections.Generic;
using System.Web.Mvc;

namespace NISC_MFP_MVC.ViewModels
{
    public class OutputReportViewModel : AbstractViewModel
    {

        public OutputReportViewModel()
        {
            departmentNames = new List<SelectListItem>();
            userNames = new List<SelectListItem>();
            cardReaders = new List<SelectListItem>();
        }
        public int reportType { get; set; }

        public int colorType { get; set; }

        public List<SelectListItem> departmentNames { get; set; }

        public List<SelectListItem> userNames { get; set; }

        public List<SelectListItem> cardReaders { get; set; }

        public int duration { get; set; }
    }
}