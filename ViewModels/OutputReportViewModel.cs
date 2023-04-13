using NISC_MFP_MVC.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.ViewModels
{
    public class OutputReportViewModel
    {
        
        public OutputReportViewModel() {
            departmentNames = new List<SelectListItem>();
            userNames = new List<SelectListItem>();
            cardReaders = new List<SelectListItem>();
        }

        public List<SelectListItem> departmentNames { get; set; }

        public List<SelectListItem> userNames { get; set; }

        public List<SelectListItem> cardReaders { get; set; }

    }
}