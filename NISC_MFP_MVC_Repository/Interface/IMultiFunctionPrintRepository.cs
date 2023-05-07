using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IMultiFunctionPrintRepository : IRepository<InitialMultiFunctionPrintRepoDTO>
    {
        IQueryable<InitialMultiFunctionPrintRepoDTO> GetMultiple(int cr_id);
    }
}
