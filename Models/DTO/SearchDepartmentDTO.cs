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
        }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [Display(Name = "部門編號")]
        public string dept_id { get; set; }

        [Display(Name = "部門名稱")]
        public string dept_name { get; set; }

        [Display(Name = "可用點數上限")]
        [Range(0, Double.MaxValue, ErrorMessage = "{0}不得小於0")]
        public int? dept_value { get; set; }

        [Display(Name = "可用遞增餘額")]
        [Range(0, Double.MaxValue, ErrorMessage = "{0}不得小於0")]
        public int? dept_month_sum { get; set; }

        [Display(Name = "狀態")]
        public string dept_usable { get; set; }

        [Display(Name = "部門管理者Email")]
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