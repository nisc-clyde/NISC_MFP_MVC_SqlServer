using NISC_MFP_MVC_Service.DTOs.Info.User;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IUserService : IService<UserInfo>
    {
        UserInfo Get(string column, string value, string operation);
        void SoftDelete();
    }
}
