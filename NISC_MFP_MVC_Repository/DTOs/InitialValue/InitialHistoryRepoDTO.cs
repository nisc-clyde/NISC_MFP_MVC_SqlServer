using System;

namespace NISC_MFP_MVC_Repository.DTOs.InitialValue
{
    public class InitialHistoryRepoDTO
    {
        public int id { get; set; }
        public string login_user_id { get; set; } = null;
        public string login_user_name { get; set; } = null;
        public string operation { get; set; } = null;
        public string affected_data { get; set; } = null;
        public DateTime? date_time { get; set; } = null;
    }
}
