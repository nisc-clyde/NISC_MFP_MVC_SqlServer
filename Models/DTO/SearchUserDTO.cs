using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$user_id, $user_password, $work_id, $user_name, $dept_id, $dept_name, $depositor, $color_enable_flag, $copy_enable_flag, $print_enable_flag, $scan_enable_flag, $fax_enable_flag, $e_mail, $serial
    public class SearchUserDTO : AbstractSearchDTO
    {
        private string _user_id;
        private string _user_password;
        private string _work_id;
        private string _user_name;
        private string _dept_id;
        private string _dept_name;
        private string _depositor;
        private string _color_enable_flag;
        private string _copy_enable_flag;
        private string _print_enable_flag;
        private string _scan_enable_flag;
        private string _fax_enable_flag;
        private string _e_mail;
        private int _serial;


        public string user_id
        {
            get { return string.IsNullOrEmpty(_user_id) ? "" : _user_id; }
            set { _user_id = value; }
        }

        public string user_password
        {
            get { return string.IsNullOrEmpty(_user_password) ? "" : _user_password; }
            set { _user_password = value; }
        }

        public string work_id
        {
            get { return string.IsNullOrEmpty(_work_id) ? "" : _work_id; }
            set { _work_id = value; }
        }

        public string user_name
        {
            get { return string.IsNullOrEmpty(_user_name) ? "" : _user_name; }
            set { _user_name = value; }
        }

        public string dept_id
        {
            get { return string.IsNullOrEmpty(_dept_id) ? "" : _dept_id; }
            set { _dept_id = value; }
        }

        public string dept_name
        {
            get { return string.IsNullOrEmpty(_dept_name) ? "" : _dept_name; }
            set { _dept_name = value; }
        }

        public string depositor
        {
            get { return string.IsNullOrEmpty(_depositor) ? "" : _depositor; }
            set { _depositor = value; }
        }

        public string color_enable_flag
        {
            get { return string.IsNullOrEmpty(_color_enable_flag) ? "" : _color_enable_flag; }
            set { _color_enable_flag = value; }
        }

        public string copy_enable_flag
        {
            get { return string.IsNullOrEmpty(_copy_enable_flag) ? "" : _copy_enable_flag; }
            set { _copy_enable_flag = value; }
        }

        public string print_enable_flag
        {
            get { return string.IsNullOrEmpty(_print_enable_flag) ? "" : _print_enable_flag; }
            set { _print_enable_flag = value; }
        }

        public string scan_enable_flag
        {
            get { return string.IsNullOrEmpty(_scan_enable_flag) ? "" : _scan_enable_flag; }
            set { _scan_enable_flag = value; }
        }

        public string fax_enable_flag
        {
            get { return string.IsNullOrEmpty(_fax_enable_flag) ? "" : _fax_enable_flag; }
            set { _fax_enable_flag = value; }
        }

        public string e_mail
        {
            get { return string.IsNullOrEmpty(_e_mail) ? "" : _e_mail; }
            set { _e_mail = value; }
        }

        public int serial { get; set; } = 0;

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "user_id";
                case "1": return "user_password";
                case "2": return "work_id";
                case "3": return "user_name";
                case "4": return "dept_id";
                case "5": return "dept_name";
                case "6": return "color_enable_flag";
                case "7": return "e_mail";
                default: return "";
            }
        }
    }
}