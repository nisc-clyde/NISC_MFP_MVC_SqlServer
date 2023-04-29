namespace NISC_MFP_MVC_Service.DTOs.Info.Department
{
    public class DepartmentInfoConvert2Text : AbstractDepartmentInfo
    {
        public override string dept_usable
        {
            get => base.dept_usable;
            set
            {
                if (value == "0")
                {
                    base.dept_usable = "停用";
                }
                else if (value == "1")
                {
                    base.dept_usable = "啟用";
                }
                else
                {
                    base.dept_usable = value;
                }
            }
        }
    }
}
