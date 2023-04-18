using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$card_id, $value, $freevalue, $user_id, $user_name, $card_type, $enable, $serial
    public class SearchCardDTO : AbstractSearchDTO
    {
        [DisplayName("卡片編號")]
        public string card_id { get; set; }
        [DisplayName("點數")]
        public int? value { get; set; }

        [DisplayName("免費點數")]
        public int freevalue { get; set; }

        [DisplayName("使用者帳號")]
        public string user_id { get; set; }

        [DisplayName("使用者姓名")]
        public string user_name { get; set; }

        [DisplayName("屬性")]
        public string card_type { get; set; }

        [DisplayName("使用狀態")]
        public string enable { get; set; }

        public int serial { get; set; }

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "card_id";
                case "1": return "value";
                case "2": return "freevalue";
                case "3": return "user_id";
                case "4": return "user_name";
                case "5": return "card_type";
                case "6": return "enable";
                default: return "";
            }
        }

    }
}