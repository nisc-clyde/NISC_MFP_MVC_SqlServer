using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = ("請輸入帳號"))]
        public string account { get; set; }

        [Required(ErrorMessage = ("請輸入密碼"))]
        public string password { get; set; }
    }
}