using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IMultiFunctionPrintService : IService<MultiFunctionPrintInfo>
    {
        IQueryable<MultiFunctionPrintInfo> GetMultiple(int cr_id);
    }
}
