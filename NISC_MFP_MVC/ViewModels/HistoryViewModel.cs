using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.ViewModels
{
    public class HistoryViewModel
    {
        public string date_time { get; set; }

        public string login_user_id { get; set; }

        public string login_user_name { get; set; }

        public string operation { get; set; }

        public string affected_data { get; set; }
    }
}