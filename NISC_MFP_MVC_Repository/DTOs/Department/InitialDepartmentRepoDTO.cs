namespace NISC_MFP_MVC_Repository.DTOs.Department
{
    public class InitialDepartmentRepoDTO
    {
        private int? _dept_value = 0;
        private int? _dept_month_sum = 0;

        public virtual int serial { get; set; }
        public virtual string dept_id { get; set; }
        public virtual string dept_name { get; set; } = null;
        public virtual int? dept_value { get { return _dept_value; } set { _dept_value = value ?? 0; } }

        public virtual int? dept_month_sum { get { return _dept_month_sum; } set { _dept_month_sum = value ?? 0; } }

        public virtual string dept_usable { get; set; } = "1";
        public virtual string dept_email { get; set; } = null;
        public virtual string if_deleted { get; set; } = "0";
    }
}