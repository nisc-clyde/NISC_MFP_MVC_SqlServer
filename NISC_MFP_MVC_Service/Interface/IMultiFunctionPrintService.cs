using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using System.Linq;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IMultiFunctionPrintService : IService<MultiFunctionPrintInfo>
    {
        /// <summary>
        /// 邏輯判斷後取得所有在cr_id下的MFP
        /// </summary>
        /// <param name="cr_id">CardReader的cr_id</param>
        /// <returns></returns>
        IQueryable<MultiFunctionPrintInfo> GetMultiple(int cr_id);

        /// <summary>
        /// 邏輯判斷後新增MFP到CardReader下
        /// </summary>
        /// <param name="instance">欲新增的MFP</param>
        /// <param name="cr_id">CardReader的cr_id</param>
        void Insert(MultiFunctionPrintInfo instance, int cr_id);

        /// <summary>
        /// 邏輯判斷後修改MFP
        /// </summary>
        /// <param name="instance">欲修改的MFP</param>
        /// <param name="cr_id">CardReader的cr_id</param>
        void Update(MultiFunctionPrintInfo instance, int cr_id);

        /// <summary>
        /// 邏輯判斷後刪除CardReader時呼叫此Method，刪除CardReader底下的所有MFP
        /// </summary>
        /// <param name="cr_id"></param>
        void DeleteMFPById(string cr_id);

    }
}
