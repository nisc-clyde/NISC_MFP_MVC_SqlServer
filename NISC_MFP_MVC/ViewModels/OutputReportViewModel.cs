using NISC_MFP_MVC.Models;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NISC_MFP_MVC.ViewModels
{
    public class OutputReportViewModel : AbstractSearchDTO
    {

        public OutputReportViewModel()
        {
            departmentNames = new List<SelectListItem>();
            userNames = new List<SelectListItem>();
            cardReaders = new List<SelectListItem>();
        }

        public List<SelectListItem> departmentNames { get; set; }

        public List<SelectListItem> userNames { get; set; }

        public List<SelectListItem> cardReaders { get; set; }

    }
}