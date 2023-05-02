namespace NISC_MFP_MVC_Service.DTOs.Info.Department
{
    public class DepartmentInfo
    {
        public string dept_id { get; set; }
        public string dept_name { get; set; }
        public int? dept_value { get; set; }
        public int? dept_month_sum { get; set; }
        public virtual string dept_usable { get; set; }
        public string dept_email { get; set; }
        public int serial { get; set; }
    }
}
