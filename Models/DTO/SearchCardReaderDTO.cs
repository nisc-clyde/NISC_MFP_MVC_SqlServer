using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$cr_id, $cr_ip, $cr_port, $cr_type, $cr_mode, $cr_card_switch, $cr_status, $serial
    public class SearchCardReaderDTO : AbstractSearchDTO
    {
        private string _cr_id;
        private string _cr_ip;
        private string _cr_port;
        private string _cr_type;
        private string _cr_mode;
        private string _cr_card_switch;
        private string _cr_status;
        //private int _serial;

        public string cr_id
        {
            get { return string.IsNullOrEmpty(_cr_id) ? "" : _cr_id; }
            set { _cr_id = value; }
        }

        public string cr_ip
        {
            get { return string.IsNullOrEmpty(_cr_ip) ? "" : _cr_ip; }
            set { _cr_ip = value; }
        }

        public string cr_port
        {
            get { return string.IsNullOrEmpty(_cr_port) ? "" : _cr_port; }
            set { _cr_port = value; }
        }

        public string cr_type
        {
            get { return string.IsNullOrEmpty(_cr_type) ? "" : _cr_type; }
            set { _cr_type = value; }
        }

        public string cr_mode
        {
            get { return string.IsNullOrEmpty(_cr_mode) ? "" : _cr_mode; }
            set { _cr_mode = value; }
        }
        public string cr_card_switch
        {
            get { return string.IsNullOrEmpty(_cr_card_switch) ? "" : _cr_card_switch; }
            set { _cr_card_switch = value; }
        }
        public string cr_status
        {
            get { return string.IsNullOrEmpty(_cr_status) ? "" : _cr_status; }
            set { _cr_status = value; }
        }

        public int serial { get; set; } = 0;

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "cr_id";
                case "1": return "cr_ip";
                case "2": return "cr_port";
                case "3": return "cr_type";
                case "4": return "cr_mode";
                case "5": return "cr_card_switch";
                case "6": return "cr_status";
                default: return "";
            }
        }
    }
}