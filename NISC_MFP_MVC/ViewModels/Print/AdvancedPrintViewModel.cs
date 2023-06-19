using System.Collections.Generic;
using System.Web.Mvc;

namespace NISC_MFP_MVC.ViewModels.Print
{
    public class AdvancedPrintViewModel
    {
        public AdvancedPrintViewModel()
        {
            departmentList = new List<SelectListItem>();
            operationList = new List<SelectListItem>();

            departmentList.Add(new SelectListItem { Text = "已選擇部門", Disabled = true });

            operationList.Add(new SelectListItem { Text = "已選擇動作", Disabled = true });
            operationList.Add(new SelectListItem { Text = "影印", Value = "C" });
            operationList.Add(new SelectListItem { Text = "列印", Value = "P" });
            operationList.Add(new SelectListItem { Text = "掃描", Value = "S" });
            operationList.Add(new SelectListItem { Text = "傳真", Value = "F" });
        }

        public List<SelectListItem> departmentList { get; set; }

        public List<SelectListItem> operationList { get; set; }
    }
}