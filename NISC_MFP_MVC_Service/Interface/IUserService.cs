using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using System.Collections.Generic;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IUserService : IService<UserInfo>
    {
        /// <summary>
        /// 邏輯判斷後刪除全部資料
        /// </summary>
        void SoftDelete();

        /// <summary>
        /// 更新User權限
        /// </summary>
        /// <param name="authority">欲更改之權限</param>
        /// <param name="user_id">欲更改之user_id</param>
        void setUserPermission(string authority, string user_id);

        /// <summary>
        /// 關鍵字查詢user_id和user_name
        /// </summary>
        /// <param name="prefix">關鍵字</param>
        /// <returns></returns>
        IEnumerable<UserInfo> SearchByIdAndName(string prefix);

        void InsertBulkData(List<UserInfo> instance);

    }
}
