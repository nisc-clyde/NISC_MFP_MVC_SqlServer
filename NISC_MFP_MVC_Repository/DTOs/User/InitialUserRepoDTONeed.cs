using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NISC_MFP_MVC_Repository.DTOs.User
{
    public class InitialUserRepoDTONeed
    {
        public string user_id { get; set; }
        public string user_password { get; set; } = "";
        public string work_id { get; set; } = "";
        public string user_name { get; set; } = "";
        public string dept_id { get; set; } = null;
        public string dept_name { get; set; } = "";
        public string color_enable_flag { get; set; } = "0";
        public string copy_enable_flag { get; set; } = "1";
        public string print_enable_flag { get; set; } = "1";
        public string scan_enable_flag { get; set; } = "1";
        public string fax_enable_flag { get; set; } = "0";
        public string e_mail { get; set; } = null;
        public int serial { get; set; } = 0;
    }
}
