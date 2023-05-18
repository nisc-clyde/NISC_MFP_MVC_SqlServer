using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

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

        [Display(Name = "姓名")]
        public string user_name { get; set; }
    }
}