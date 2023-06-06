using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.ViewModels.User.UserAreas
{
    public class UserEditViewModel
    {
        [Display(Name = "帳號")]
        public string user_id { get; set; }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [Display(Name = "目前密碼")]
        public string user_password { get; set; }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [Display(Name = "新密碼")]
        public string new_user_password { get; set; }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [Compare("new_user_password", ErrorMessage = "密碼不一致")]
        [Display(Name = "新密碼確認")]
        public string new_user_password_confirm { get; set; }

    }
}