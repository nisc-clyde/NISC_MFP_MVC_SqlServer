using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.DTOs.OutputReport;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IPrintRepository : IRepository<InitialPrintRepoDTO>
    {
        IQueryable<InitialPrintRepoDTO> GetRecord(InitialOutputReportRepoDTO initialOutputReportRepoDTO);
    }
}
