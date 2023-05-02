using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IPrintRepository : IRepository<InitialPrintRepoDTO>
    {
    }
}
