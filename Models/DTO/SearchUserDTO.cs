using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$user_id, $user_password, $work_id, $user_name, $dept_id, $dept_name, $depositor, $color_enable_flag, $copy_enable_flag, $print_enable_flag, $scan_enable_flag, $fax_enable_flag, $e_mail, $serial
    public class SearchUserDTO : AbstractSearchDTO
    {
        public SearchUserDTO()
        {
            user_id = "";
            user_password = "";
            work_id = "";
            user_name = "";
            dept_id = "";
            dept_name = "";
            depositor = "";
            color_enable_flag = "";
            copy_enable_flag = "";
            print_enable_flag = "";
            scan_enable_flag = "";
            fax_enable_flag = "";
            e_mail = "";
            serial = 0;
        }

        [Required(ErrorMessage = "此欄位為必填資料")]
        public string user_id { get; set; }

        public string user_password { get; set; }

        public string work_id { get; set; }

        public string user_name { get; set; }

        public string dept_id { get; set; }

        public string dept_name { get; set; }

        public string depositor { get; set; }

        public string color_enable_flag { get; set; }

        public string copy_enable_flag { get; set; }

        public string print_enable_flag { get; set; }

        public string scan_enable_flag { get; set; }

        public string fax_enable_flag { get; set; }

        public string e_mail { get; set; }

        public int serial { get; set; } = 0;

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "user_id";
                case "1": return "user_password";
                case "2": return "work_id";
                case "3": return "user_name";
                case "4": return "dept_id";
                case "5": return "dept_name";
                case "6": return "color_enable_flag";
                case "7": return "e_mail";
                default: return "";
            }
        }
    }
}