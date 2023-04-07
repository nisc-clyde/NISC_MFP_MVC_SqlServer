using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    public class SearchHistoryDTO : AbstractSearchDTO
    {
        public string date_time { get; set; }
        public string login_user_id { get; set; }
        public string login_user_name { get; set; }
        public string operation { get; set; }
        public string affected_data { get; set; }
    }
}