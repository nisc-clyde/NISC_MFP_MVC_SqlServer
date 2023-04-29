namespace NISC_MFP_MVC_Service.DTOs.Info.Department
{
    public abstract class AbstractDepartmentInfo
    {
        public virtual string dept_id { get; set; }
        public virtual string dept_name { get; set; }
        public virtual int? dept_value { get; set; }
        public virtual int? dept_month_sum { get; set; }
        public virtual string dept_usable { get; set; }
        public virtual string dept_email { get; set; }
        public virtual int serial { get; set; }
    }
}
