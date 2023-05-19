using NISC_MFP_MVC_Repository.DTOs.Department;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IDepartmentRepository : IRepository<InitialDepartmentRepoDTO>
    {
        /// <summary>
        /// 刪除全部資料
        /// </summary>
        void SoftDelete();

    }
}
