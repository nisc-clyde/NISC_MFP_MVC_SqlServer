using NISC_MFP_MVC.Models;
using System;

namespace NISC_MFP_MVC.ViewModels
{
    public class PrintViewModel : AbstractSearchDTO
    {
        public string mfp_name { get; set; }

        public string user_name { get; set; }

        public string dept_name { get; set; }

        public string card_id { get; set; }

        public string card_type { get; set; }

        public string usage_type { get; set; }

        public string page_color { get; set; }

        public int? page { get; set; }

        public int? value { get; set; }

        public string print_date { get; set; }

        public string document_name { get; set; }

        public int serial { get; set; }

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