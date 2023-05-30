using NISC_MFP_MVC_Repository.DTOs.User;
using System.Collections.Generic;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IUserRepository : IRepository<InitialUserRepoDTO>
    {
        /// <summary>
        /// 刪除全部資料
        /// </summary>
        void SoftDelete();

        void InsertBulkData(List<InitialUserRepoDTO> instance);

    }
}
