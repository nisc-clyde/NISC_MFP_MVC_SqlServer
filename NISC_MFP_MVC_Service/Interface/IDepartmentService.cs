using NISC_MFP_MVC_Service.DTOs.Info.Department;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IDepartmentService : IService<DepartmentInfo>
    {
        DepartmentInfo Get(string column, string value, string operation);
        void SoftDelete();
    }
}
