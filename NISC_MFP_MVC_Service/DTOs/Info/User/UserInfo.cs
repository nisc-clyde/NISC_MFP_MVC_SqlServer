namespace NISC_MFP_MVC_Service.DTOs.Info.User
{
    public class UserInfo
    {
        public virtual string user_id { get; set; }
        public virtual string user_password { get; set; }
        public virtual string work_id { get; set; }
        public virtual string user_name { get; set; }
        public virtual string dept_id { get; set; }
        public virtual string dept_name { get; set; }
        public virtual string color_enable_flag { get; set; }
        public virtual string copy_enable_flag { get; set; }
        public virtual string print_enable_flag { get; set; }
        public virtual string scan_enable_flag { get; set; }
        public virtual string fax_enable_flag { get; set; }
        public virtual string e_mail { get; set; }
        public virtual int serial { get; set; }
    }
}
