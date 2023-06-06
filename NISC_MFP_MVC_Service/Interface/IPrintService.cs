using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Print;
using NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print;
using System.Collections.Generic;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IPrintService : IService<PrintInfo>
    {
        /// <summary>
        /// 取得所有user_id的半年內之影列印紀錄
        /// </summary>
        /// <param name="dataTableRequest">DataTable Request</param>
        /// <param name="user_id">欲查詢之user_id</param>
        /// <returns></returns>
        List<RecentlyPrintRecord> GetRecentlyPrintRecord(DataTableRequest dataTableRequest, string user_id);
    }
}
