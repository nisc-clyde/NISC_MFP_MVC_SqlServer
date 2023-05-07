using System.Collections.Generic;
using System.Web.Mvc;

namespace NISC_MFP_MVC.ViewModels.Print
{
    public class AdvancedPrintViewModel
    {
        private List<SelectListItem> _departmentList;
        private List<SelectListItem> _operationList;

        public AdvancedPrintViewModel()
        {
            _departmentList = new List<SelectListItem>();
            _operationList = new List<SelectListItem>();

            _departmentList.Add(new SelectListItem { Text = "已選擇部門", Disabled = true });

            _operationList.Add(new SelectListItem { Text = "已選擇動作", Disabled = true });
            _operationList.Add(new SelectListItem { Text = "影印", Value = "C" });
            _operationList.Add(new SelectListItem { Text = "列印", Value = "P" });
            _operationList.Add(new SelectListItem { Text = "掃描", Value = "S" });
            _operationList.Add(new SelectListItem { Text = "傳真", Value = "F" });
        }

        public List<SelectListItem> departmentList
        {
            get { return _departmentList; }
            set { _departmentList = value; }
        }

        public List<SelectListItem> operationList
        {
            get { return _operationList; }
            set { _operationList = value; }
        }

    }
}