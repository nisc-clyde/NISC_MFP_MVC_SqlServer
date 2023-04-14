using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO_Initial
{
    public class tb_user_dto
    {
        public string user_id { get; set; }
        public string user_password { get; set; } = null;
        public string work_id { get; set; } = null;
        public string user_name { get; set; } = null;
        public string authority { get; set; } = null;
        public string depositor { get; set; } = "0";
        public string dept_id { get; set; } = null;
        public string import_flag { get; set; } = "0";
        public string color_enable_flag { get; set; } = "0";
        public string reserve_flag { get; set; } = "0";
        public string scan_reserve_flag { get; set; } = "0";
        public string personal_water_flag { get; set; } = "0";
        public string personal_water_depth { get; set; } = "3";
        public string e_mail { get; set; } = null;
        public string if_deleted { get; set; } = "0";
        public string group_id { get; set; } = "gp_01";
        public string copy_enable_flag { get; set; } = "1";
        public string print_enable_flag { get; set; } = "1";
        public string scan_enable_flag { get; set; } = "1";
        public string fax_enable_flag { get; set; } = "0";

        public tb_user Convert2DatabaseModel()
        {
            tb_user user = new tb_user();

            user.user_id = user_id;
            user.user_password = user_password;
            user.work_id = work_id;
            user.user_name = user_name;
            user.authority = authority;
            user.depositor = depositor;
            user.dept_id = dept_id;
            user.import_flag = import_flag;
            user.color_enable_flag = color_enable_flag;
            user.reserve_flag = reserve_flag;
            user.scan_reserve_flag = scan_reserve_flag;
            user.personal_water_flag = personal_water_flag;
            user.personal_water_depth = personal_water_depth;
            user.e_mail = e_mail;
            user.if_deleted = if_deleted;
            user.group_id = group_id;
            user.copy_enable_flag = copy_enable_flag;
            user.print_enable_flag = print_enable_flag;
            user.scan_enable_flag = scan_enable_flag;
            user.fax_enable_flag = fax_enable_flag;

            return user;
        }
    }
}