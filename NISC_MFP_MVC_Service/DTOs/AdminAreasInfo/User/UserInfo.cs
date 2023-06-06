namespace NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User
{
    public class UserInfo
    {
        public string user_id { get; set; }
        public string user_password { get; set; }
        public string work_id { get; set; }
        public string user_name { get; set; }
        public string authority { get; set; }
        public string dept_id { get; set; }
        public string dept_name { get; set; }
        public string color_enable_flag { get; set; }
        public string copy_enable_flag { get; set; }
        public string print_enable_flag { get; set; }
        public string scan_enable_flag { get; set; }
        public string fax_enable_flag { get; set; }
        public string e_mail { get; set; }
        public int serial { get; set; }
    }
}
