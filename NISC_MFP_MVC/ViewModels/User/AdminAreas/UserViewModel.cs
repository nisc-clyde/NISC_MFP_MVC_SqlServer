﻿using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.ViewModels.User.AdminAreas
{
    //$user_id, $user_password, $work_id, $user_name, $dept_id, $dept_name, $depositor, $color_enable_flag, $copy_enable_flag, $print_enable_flag, $scan_enable_flag, $fax_enable_flag, $e_mail, $serial
    public class UserViewModel : AbstractViewModel
    {
        [Required(ErrorMessage = "此欄位為必填資料")]
        [Display(Name = "帳號（請注意，帳號一旦新增即不可修改）")]
        public string user_id { get; set; }

        [Display(Name = "密碼")]
        public string user_password { get; set; }

        [Display(Name = "工號")]
        public string work_id { get; set; }

        [Display(Name = "姓名")]
        public string user_name { get; set; }

        public string authority { get; set; }

        [Display(Name = "部門編號")]
        public string dept_id { get; set; }

        [Display(Name = "部門名稱")]
        public string dept_name { get; set; }

        [Display(Name = "彩色使用權限")]
        public string color_enable_flag { get; set; }

        [Display(Name = "影印使用權限")]
        public string copy_enable_flag { get; set; }

        [Display(Name = "列印使用權限")]
        public string print_enable_flag { get; set; }

        [Display(Name = "掃描使用權限")]
        public string scan_enable_flag { get; set; }

        [Display(Name = "傳真使用權限")]
        public string fax_enable_flag { get; set; }

        [RegularExpression("^\\S+@\\S+\\.\\S+$", ErrorMessage = "此欄位格式不正確")]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "信箱")]
        public string e_mail { get; set; }

        public string operation { get; set; }

        public int serial { get; set; } = 0;
    }
}