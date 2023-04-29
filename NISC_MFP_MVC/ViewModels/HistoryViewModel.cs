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

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "date_time";
                case "1": return "login_user_id";
                case "2": return "login_user_name";
                case "3": return "operation";
                case "4": return "affected_data";
                default: return "date_time";
            }
        }
    }
}