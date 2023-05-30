using NISC_MFP_MVC_Repository.DTOs.Card;
using System.Collections;
using System.Collections.Generic;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface ICardRepository : IRepository<InitialCardRepoDTO>
    {
        /// <summary>
        /// 負責重置免費點數
        /// </summary>
        /// <param name="freevalue">重置後點數</param>
        void UpdateResetFreeValue(int freevalue);

        /// <summary>
        /// 負責儲值點數
        /// </summary>
        /// <param name="value">儲值後的值</param>
        /// <param name="serial">欲儲值的卡之serial</param>
        void UpdateDepositValue(int value, int serial);

        /// <summary>
        /// 刪除全部資料
        /// </summary>
        void SoftDelete();

        void InsertBulkData(List<InitialCardRepoDTO> instance);

    }
}
