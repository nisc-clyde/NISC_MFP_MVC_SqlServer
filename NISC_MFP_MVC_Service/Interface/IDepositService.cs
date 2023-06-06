using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Deposit;
using NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print;
using System.Collections.Generic;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IDepositService : IService<DepositInfo>
    {
        /// <summary>
        /// 取得所有user_id的半年內之儲值紀錄
        /// </summary>
        /// <param name="dataTableRequest">DataTable Request</param>
        /// <param name="user_id">欲查詢之user_id</param>
        /// <returns></returns>
        List<RecentlyDepositRecord> GetRecentlyDepositRecord(DataTableRequest dataTableRequest, string user_id);

    }
}
