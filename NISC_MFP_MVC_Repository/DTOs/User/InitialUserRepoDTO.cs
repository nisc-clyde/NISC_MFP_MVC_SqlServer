namespace NISC_MFP_MVC_Repository.DTOs.User
{
    public class InitialUserRepoDTO
    {
        public int serial { get; set; }
        public string user_id { get; set; }
        public string user_password { get; set; } = "";
        public string work_id { get; set; } = "";
        public string user_name { get; set; } = "";
        public string authority { get; set; } = "";
        public string depositor { get; set; } = "0";
        public string dept_id { get; set; } = null;

        public string dept_name { get; set; } = "";

        public string import_flag { get; set; } = "0";
        public virtual string color_enable_flag { get; set; } = "0";
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
    }
}
