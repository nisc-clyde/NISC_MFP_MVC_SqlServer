using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Implement;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface ICardRepository : IRepository<InitialCardRepoDTO>
    {
        void UpdateResetFreeValue(int freevalue);
        void UpdateDepositValue(int value, int serial);
        void SoftDelete();

    }
}
