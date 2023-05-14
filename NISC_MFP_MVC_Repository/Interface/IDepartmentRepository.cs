using NISC_MFP_MVC_Repository.DTOs.Department;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IDepartmentRepository : IRepository<InitialDepartmentRepoDTO>
    {
        InitialDepartmentRepoDTO Get(string column, string value, string operation);
        void SoftDelete();

    }
}
