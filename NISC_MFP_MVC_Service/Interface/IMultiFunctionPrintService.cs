﻿using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using System.Linq;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IMultiFunctionPrintService : IService<MultiFunctionPrintInfo>
    {
        IQueryable<MultiFunctionPrintInfo> GetMultiple(int cr_id);
    }
}