using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using System.Collections.Generic;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IDepartmentService : IService<DepartmentInfo>
    {
        /// <summary>
        /// 邏輯判斷後刪除全部資料
        /// </summary>
        void SoftDelete();

        void InsertBulkData(List<DepartmentInfo> instance);
    }
}
