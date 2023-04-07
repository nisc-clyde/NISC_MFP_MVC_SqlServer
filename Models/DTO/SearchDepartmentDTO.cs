using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$dept_id,$dept_name,$dept_value,$dept_month_sum,$dept_usable,$dept_email,$serial
    public class SearchDepartmentDTO : AbstractSearchDTO
    {
        public string dept_id { get; set; }
        public string dept_name { get; set; }
        public int? dept_value { get; set; }
        public int? dept_month_sum { get; set; }
        public string dept_usable { get; set; }
        public string dept_email { get; set; }
        public int serial { get; set; }
    }
}