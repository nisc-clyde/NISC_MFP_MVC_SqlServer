using NISC_MFP_MVC.Models;
using System;

namespace NISC_MFP_MVC.ViewModels
{
    public class DepositViewModel : AbstractSearchDTO
    {

        public string user_name { get; set; }

        public string user_id { get; set; }

        public string card_id { get; set; }

        public string card_user_id { get; set; }

        public string card_user_name { get; set; }

        public int? pbalance { get; set; }

        public int? deposit_value { get; set; }

        public int? final_value { get; set; }

        public string deposit_date { get; set; }

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "user_name";
                case "1": return "user_id";
                case "2": return "card_id";
                case "3": return "card_user_id";
                case "4": return "card_user_name";
                case "5": return "pbalance";
                case "6": return "deposit_value";
                case "7": return "final_value";
                case "8": return "deposit_date";
                default: return "";
            }
        }

    }
}