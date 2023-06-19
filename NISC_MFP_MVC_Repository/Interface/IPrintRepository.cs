using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.OutputReport;
using NISC_MFP_MVC_Repository.DTOs.Print;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IPrintRepository : IRepository<InitialPrintRepoDTO>
    {
        /// <summary>
        /// OutputReport對tb_logs_print進行Filter後取得結果
        /// </summary>
        /// <param name="initialOutputReportRepoDTO">Filter</param>
        /// <returns></returns>
        IQueryable<InitialPrintRepoDTONeed> GetRecord(InitialOutputReportRepoDTO initialOutputReportRepoDTO);
    }
}
