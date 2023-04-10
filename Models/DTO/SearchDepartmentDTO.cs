using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$dept_id,$dept_name,$dept_value,$dept_month_sum,$dept_usable,$dept_email,$serial
    public class SearchDepartmentDTO : AbstractSearchDTO
    {
        private string _dept_id;
        private string _dept_name;
        //private int _dept_value;
        //private int _dept_month_sum;
        private string _dept_usable;
        private string _dept_email;
        //private int _serial;

        public string dept_id
        {
            get { return string.IsNullOrEmpty(_dept_id) ? "" : _dept_id; }
            set { _dept_id = value; }
        }
        public string dept_name
        {
            get { return string.IsNullOrEmpty(_dept_name) ? "" : _dept_name; }
            set { _dept_name = value; }
        }
        public int? dept_value { get; set; } = 0;
        public int? dept_month_sum { get; set; } = 0;
        public string dept_usable
        {
            get { return string.IsNullOrEmpty(_dept_usable) ? "" : _dept_usable; }
            set { _dept_usable = value; }
        }
        public string dept_email
        {
            get { return string.IsNullOrEmpty(_dept_email) ? "" : _dept_email; }
            set { _dept_email = value; }
        }
        public int serial { get; set; } = 0;

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