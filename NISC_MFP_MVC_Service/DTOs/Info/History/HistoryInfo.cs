using System;

namespace NISC_MFP_MVC_Service.DTOs.Info.History
{
    public class HistoryInfo
    {
        public int id { get; set; }
        public string login_user_id { get; set; }
        public string login_user_name { get; set; }
        public string operation { get; set; }
        public string affected_data { get; set; }
        public string date_time { get; set; }
    }
}
