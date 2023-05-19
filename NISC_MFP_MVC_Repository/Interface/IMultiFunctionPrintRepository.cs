using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IMultiFunctionPrintRepository : IRepository<InitialMultiFunctionPrintRepoDTO>
    {
        /// <summary>
        /// 取得所有在cr_id下的MFP
        /// </summary>
        /// <param name="cr_id">CardReader的cr_id</param>
        /// <returns></returns>
        IQueryable<InitialMultiFunctionPrintRepoDTO> GetMultiple(int cr_id);

        /// <summary>
        /// 刪除CardReader時呼叫此Method，刪除CardReader底下的所有MFP
        /// </summary>
        /// <param name="cr_id"></param>
        void DeleteMFPById(string cr_id);

    }
}
