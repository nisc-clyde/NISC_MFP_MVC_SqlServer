using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IPrintRepository : IRepository<InitialPrintRepoDTO>
    {
    }
}
