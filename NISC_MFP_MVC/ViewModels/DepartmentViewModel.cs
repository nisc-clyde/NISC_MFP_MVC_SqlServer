using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.ViewModels
{
    public class DepartmentViewModel : AbstractViewModel
    {
        [Required(ErrorMessage = "此欄位為必填資料")]
        [Display(Name = "部門編號")]
        public string dept_id { get; set; }

        [Display(Name = "部門名稱")] public string dept_name { get; set; }

        [Display(Name = "可用點數上限")]
        [Range(0, double.MaxValue, ErrorMessage = "{0}不得小於0")]
        public int? dept_value { get; set; }

        [Display(Name = "可用遞增餘額")]
        [Range(0, double.MaxValue, ErrorMessage = "{0}不得小於0")]
        public int? dept_month_sum { get; set; }

        [Display(Name = "狀態")] public string dept_usable { get; set; }

        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage = "此欄位格式不正確")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "部門管理者Email")]
        public string dept_email { get; set; }

        public int serial { get; set; }
    }
}