using AutoMapper;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IMultiFunctionPrintRepository : IRepository<InitialMultiFunctionPrintRepoDTO>
    {
        IQueryable<InitialMultiFunctionPrintRepoDTO> GetMultiple(int cr_id);
    }
}
