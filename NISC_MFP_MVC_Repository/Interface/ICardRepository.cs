using NISC_MFP_MVC_Repository.DTOs.Card;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface ICardRepository : IRepository<InitialCardRepoDTO>
    {
        void UpdateResetFreeValue(int freevalue);

        void UpdateDepositValue(int value, int serial);

    }
}
