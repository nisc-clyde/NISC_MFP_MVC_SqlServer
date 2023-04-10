using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    public class SearchPrintDTO : AbstractSearchDTO
    {
        private string _mfp_name;
        private string _user_name;
        private string _dept_name;
        private string _card_id;
        private string _card_type;
        private string _usage_type;
        private string _page_color;
        //private int _page;
        //private int _value;
        private string _print_date;
        private string _document_name;
        public string mfp_name
        {
            get { return string.IsNullOrEmpty(_mfp_name) ? "" : _mfp_name; }
            set { _mfp_name = value; }
        }
        public string user_name
        {
            get { return string.IsNullOrEmpty(_user_name) ? "" : _user_name; }
            set { _user_name = value; }
        }
        public string dept_name
        {
            get { return string.IsNullOrEmpty(_dept_name) ? "" : _dept_name; }
            set { _dept_name = value; }
        }
        public string card_id
        {
            get { return string.IsNullOrEmpty(_card_id) ? "" : _card_id; }
            set { _card_id = value; }
        }
        public string card_type
        {
            get { return string.IsNullOrEmpty(_card_type) ? "" : _card_type; }
            set { _card_type = value; }
        }
        public string usage_type
        {
            get { return string.IsNullOrEmpty(_usage_type) ? "" : _usage_type; }
            set { _usage_type = value; }
        }
        public string page_color
        {
            get { return string.IsNullOrEmpty(_page_color) ? "" : _page_color; }
            set { _page_color = value; }
        }
        public int? page { get; set; } = 0;
        public int? value { get; set; } = 0;
        public string print_date
        {
            get { return string.IsNullOrEmpty(_print_date) ? "" : _print_date; }
            set { _print_date = value; }
        }
        public string document_name
        {
            get { return string.IsNullOrEmpty(_document_name) ? "" : _document_name; }
            set { _document_name = value; }
        }

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "mfp_name";
                case "1": return "user_name";
                case "2": return "dept_name";
                case "3": return "card_id";
                case "4": return "card_type";
                case "5": return "usage_type";
                case "6": return "page_color";
                case "7": return "page";
                case "8": return "value";
                case "9": return "print_date";
                case "10": return "document_name";
                default: return "print_date";
            }
        }

    }
}