using NISC_MFP_MVC_Service.DTOs.Info.Card;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface ICardService : IService<CardInfo>
    {
        /// <summary>
        /// 邏輯判斷後負責重置免費點數
        /// </summary>
        /// <param name="freevalue">重置後點數</param>
        void UpdateResetFreeValue(int freevalue);

        /// <summary>
        /// 邏輯判斷後負責儲值點數
        /// </summary>
        /// <param name="value">儲值後的值</param>
        /// <param name="serial">欲儲值的卡之serial</param>
        void UpdateDepositValue(int value, int serial);

        /// <summary>
        /// 邏輯判斷後刪除全部資料
        /// </summary>
        void SoftDelete();
    }
}
