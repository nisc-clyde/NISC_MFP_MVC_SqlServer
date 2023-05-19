using NISC_MFP_MVC_Service.DTOs.Info.User;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IUserService : IService<UserInfo>
    {
        /// <summary>
        /// 邏輯判斷後刪除全部資料
        /// </summary>
        void SoftDelete();
    }
}
