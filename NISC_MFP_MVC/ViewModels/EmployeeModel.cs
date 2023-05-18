using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NISC_MFP_MVC.ViewModels
{
    public class EmployeeModel
    {
        public EmployeeModel(string card_id, string dept_id, string dept_name, string user_name, string user_id, string work_id, string card_type, string enable, string e_mail)
        {
            this.card_id = card_id;
            this.dept_id = dept_id;
            this.dept_name = dept_name;
            this.user_name = user_name;
            this.user_id = user_id;
            this.work_id = work_id;
            this.card_type = card_type;
            this.enable = enable;
            this.e_mail = e_mail;
        }

        [DisplayName("卡片編號")]
        public string card_id { get; set; }

        [Display(Name = "部門編號")]
        public string dept_id { get; set; }

        [Display(Name = "部門名稱")]
        public string dept_name { get; set; }

        [Display(Name = "姓名")]
        public string user_name { get; set; }

        [Display(Name = "帳號（請注意，帳號一旦新增即不可修改）")]
        public string user_id { get; set; }

        [Display(Name = "工號")]
        public string work_id { get; set; } = "";

        [DisplayName("屬性")]
        public string card_type { get; set; }

        [DisplayName("使用狀態")]
        public string enable { get; set; }

        [Display(Name = "信箱")]
        public string e_mail { get; set; }
    }
}
