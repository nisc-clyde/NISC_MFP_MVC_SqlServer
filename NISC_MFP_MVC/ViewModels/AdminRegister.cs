using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.ViewModels
{
    public class AdminRegister
    {
        [Required(ErrorMessage = "此欄位為必填資料")]
        [Display(Name = "帳號")]
        public string user_id { get; set; }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [Display(Name = "密碼")]
        public string user_password { get; set; }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [Compare("user_password", ErrorMessage = "密碼不一致")]
        [Display(Name = "密碼確認")]
        public string user_password_confirm { get; set; }

        [Display(Name = "姓名")]
        public string user_name { get; set; }
    }
}