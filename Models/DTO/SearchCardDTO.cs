using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$card_id, $value, $freevalue, $user_id, $user_name, $card_type, $enable, $serial
    public class SearchCardDTO : AbstractSearchDTO
    {
        private string _card_id;
        //private int? _value;
        //private int _freevalue;
        private string _user_id;
        private string _user_name;
        private string _card_type;
        private string _enable;

        public string card_id
        {
            get
            { return string.IsNullOrEmpty(_card_id) ? "" : _card_id; }
            set
            { _card_id = value; }
        }

        public int? value { get; set; } = 0;

        public int freevalue { get; set; } = 0;

        public string user_id
        {
            get
            { return string.IsNullOrEmpty(_user_id) ? "" : _user_id; }
            set
            { _user_id = value; }
        }

        public string user_name
        {
            get
            { return string.IsNullOrEmpty(_user_name) ? "" : _user_name; }
            set
            { _user_name = value; }
        }

        public string card_type
        {
            get
            { return string.IsNullOrEmpty(_card_type) ? "" : _card_type; }
            set
            { _card_type = value; }
        }

        public string enable
        {
            get
            { return string.IsNullOrEmpty(_enable) ? "" : _enable; }
            set
            { _enable = value; }
        }

        public int serial { get; set; } = 0;

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