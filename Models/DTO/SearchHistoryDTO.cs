using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    public class SearchHistoryDTO : AbstractSearchDTO
    {
        private string _date_time;
        private string _login_user_id;
        private string _login_user_name;
        private string _operation;
        private string _affected_data;


        public string date_time
        {
            get { return string.IsNullOrEmpty(_date_time) ? "" : _date_time; }
            set { _date_time = value; }
        }
        public string login_user_id
        {
            get { return string.IsNullOrEmpty(_login_user_id) ? "" : _login_user_id; }
            set { _login_user_id = value; }
        }
        public string login_user_name
        {
            get { return string.IsNullOrEmpty(_login_user_name) ? "" : _login_user_name; }
            set { _login_user_name = value; }
        }
        public string operation
        {
            get { return string.IsNullOrEmpty(_operation) ? "" : _operation; }
            set { _operation = value; }
        }
        public string affected_data
        {
            get { return string.IsNullOrEmpty(_affected_data) ? "" : _affected_data; }
            set { _affected_data = value; }
        }

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