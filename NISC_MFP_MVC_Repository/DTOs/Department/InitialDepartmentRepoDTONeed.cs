namespace NISC_MFP_MVC_Repository.DTOs.Department
{
    public class InitialDepartmentRepoDTONeed
    {
        private int? _dept_value = 0;
        private int? _dept_month_sum = 0;

        public string dept_id { get; set; }
        public string dept_name { get; set; } = null;
        public int? dept_value { get { return _dept_value; } set { _dept_value = value ?? 0; } }
        public int? dept_month_sum { get { return _dept_month_sum; } set { _dept_month_sum = value ?? 0; } }
        public string dept_usable { get; set; } = "1";
        public string dept_email { get; set; } = null;
        public int serial { get; set; }
    }
}
