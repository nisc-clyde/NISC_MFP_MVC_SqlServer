using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$dept_id,$dept_name,$dept_value,$dept_month_sum,$dept_usable,$dept_email,$serial
    public class SearchDepartmentDTO : AbstractSearchDTO
    {
        public SearchDepartmentDTO()
        {
            dept_id = "";
            dept_name = "";
            dept_value = 0;
            dept_month_sum = 0;
            dept_usable = "";
            dept_email = "";
            serial = 0;
        }

        [Required(ErrorMessage = "此欄位為必填資料")]
        public string dept_id { get; set; }

        public string dept_name { get; set; }

        public int? dept_value { get; set; }

        public int? dept_month_sum { get; set; }
        public string dept_usable { get; set; }
        public string dept_email { get; set; }

        public int serial { get; set; }

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "dept_id";
                case "1": return "dept_name";
                case "2": return "dept_value";
                case "3": return "dept_month_sum";
                case "4": return "dept_usable";
                case "5": return "dept_email";
                default: return "";
            }
        }
    }
}