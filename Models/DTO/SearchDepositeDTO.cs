using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    public class SearchDepositeDTO : AbstractSearchDTO
    {
        private string _user_name;
        private string _user_id;
        private string _card_id;
        private string _card_user_id;
        private string _card_user_name;
        //private int _pbalance;
        //private int _deposit_value;
        //private int _final_value;
        private string _deposit_date;

        public string user_name
        {
            get { return string.IsNullOrEmpty(_user_name) ? "" : _user_name; }
            set { _user_name = value; }
        }
        public string user_id
        {
            get { return string.IsNullOrEmpty(_user_id) ? "" : _user_id; }
            set { _user_id = value; }
        }
        public string card_id
        {
            get { return string.IsNullOrEmpty(_card_id) ? "" : _card_id; }
            set { _card_id = value; }
        }
        public string card_user_id
        {
            get { return string.IsNullOrEmpty(_card_user_id) ? "" : _card_user_id; }
            set { _card_user_id = value; }
        }
        public string card_user_name
        {
            get { return string.IsNullOrEmpty(_card_user_name) ? "" : _card_user_name; }
            set { _card_user_name = value; }
        }
        public int? pbalance { get; set; } = 0;
        public int? deposit_value { get; set; } = 0;
        public int? final_value { get; set; } = 0;
        public string deposit_date
        {
            get { return string.IsNullOrEmpty(_deposit_date) ? "" : _deposit_date; }
            set { _deposit_date = value; }
        }

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