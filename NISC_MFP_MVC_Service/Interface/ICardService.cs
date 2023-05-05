using NISC_MFP_MVC_Service.DTOs.Info.Card;
using System;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface ICardService : IService<CardInfo>
    {
        void UpdateResetFreeValue(int freevalue);
        void UpdateDepositValue(int value, int serial);

    }
}
