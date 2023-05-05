using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;

namespace NISC_MFP_MVC_Common
{
    public class DataTableRequest
    {
        public DataTableRequest(NameValueCollection form)
        {
            Draw = Convert.ToInt32(form["draw"]);
            Start = Convert.ToInt32(form["start"]);
            Length = Convert.ToInt32(form["length"]);
            GlobalSearchValue = form["search[value]"];
            SortColumnIndex = form["order[0][column]"];
            SortColumnName = form["columns[" + form["order[0][column]"] + "][name]"];
            SortDirection = form["order[0][dir]"];
            RecordsTotalGet = 0;
            RecordsFilteredGet = 0;
            PostPageFrom = form["page"];

            //Filter data by column
            ColumnSearch_0 = form["columns[0][search][value]"];
            ColumnSearch_1 = form["columns[1][search][value]"];
            ColumnSearch_2 = form["columns[2][search][value]"];
            ColumnSearch_3 = form["columns[3][search][value]"];
            ColumnSearch_4 = form["columns[4][search][value]"];
            ColumnSearch_5 = form["columns[5][search][value]"];
            ColumnSearch_6 = form["columns[6][search][value]"];
            ColumnSearch_7 = form["columns[7][search][value]"];
            ColumnSearch_8 = form["columns[8][search][value]"];
            ColumnSearch_9 = form["columns[9][search][value]"];
            ColumnSearch_10 = form["columns[10][search][value]"];
        }


        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string GlobalSearchValue { get; set; }

        public string SortColumnIndex { get; set; }

        public string SortColumnName { get; set; }

        public string SortColumnProperty
        {
            get
            {
                switch (PostPageFrom)
                {

                    case "print":
                        switch (SortColumnIndex)
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
                    case "deposite":
                        switch (SortColumnIndex) {
                            case "0": return "user_name";
                            case "1": return "user_id";
                            case "2": return "card_id";
                            case "3": return "card_user_id";
                            case "4": return "card_user_name";
                            case "5": return "pbalance";
                            case "6": return "deposit_value";
                            case "7": return "final_value";
                            case "8": return "deposit_date";
                            default: return "deposit_date";
                        }
                    case "department":
                        switch (SortColumnIndex)
                        {
                            case "0": return "dept_id";
                            case "1": return "dept_name";
                            case "2": return "dept_value";
                            case "3": return "dept_month_sum";
                            case "4": return "dept_usable";
                            case "5": return "dept_email";
                            default: return "dept_id";
                        }
                    case "user":
                        switch (SortColumnIndex)
                        {
                            case "0": return "user_id";
                            case "1": return "user_password";
                            case "2": return "work_id";
                            case "3": return "user_name";
                            case "4": return "dept_id";
                            case "5": return "dept_name";
                            case "6": return "color_enable_flag";
                            case "7": return "e_mail";
                            default: return "user_id";
                        }
                    case "cardreader":
                        switch (SortColumnIndex)
                        {
                            case "0": return "cr_id";
                            case "1": return "cr_ip";
                            case "2": return "cr_port";
                            case "3": return "cr_type";
                            case "4": return "cr_mode";
                            case "5": return "cr_card_switch";
                            case "6": return "cr_status";
                            default: return "cr_id";
                        }
                    case "card":
                        switch (SortColumnIndex)
                        {
                            case "0": return "card_id";
                            case "1": return "value";
                            case "2": return "freevalue";
                            case "3": return "user_id";
                            case "4": return "user_name";
                            case "5": return "card_type";
                            case "6": return "enable";
                            default: return "card_id";
                        }
                    case "watermark":
                        switch (SortColumnIndex)
                        {
                            case "0": return "type";
                            case "1": return "left_offset";
                            case "2": return "right_offset";
                            case "3": return "top_offset";
                            case "4": return "bottom_offset";
                            case "5": return "position_mode";
                            case "6": return "fill_mode";
                            case "7": return "text";
                            case "8": return "image_path";
                            case "9": return "rotation";
                            case "10": return "color";
                            default: return "type";
                        }
                    case "history":
                        switch (SortColumnIndex)
                        {
                            case "0": return "date_time";
                            case "1": return "login_user_id";
                            case "2": return "login_user_name";
                            case "3": return "operation";
                            case "4": return "affected_data";
                            default: return "date_time";
                        }
                    default: return "";
                }
            }
        }

        public string SortDirection { get; set; }

        public int RecordsTotalGet { get; set; }

        public int RecordsFilteredGet { get; set; }

        public string PostPageFrom { get; set; }

        public string ColumnSearch_0 { get; set; }
        public string ColumnSearch_1 { get; set; }
        public string ColumnSearch_2 { get; set; }
        public string ColumnSearch_3 { get; set; }
        public string ColumnSearch_4 { get; set; }
        public string ColumnSearch_5 { get; set; }
        public string ColumnSearch_6 { get; set; }
        public string ColumnSearch_7 { get; set; }
        public string ColumnSearch_8 { get; set; }
        public string ColumnSearch_9 { get; set; }
        public string ColumnSearch_10 { get; set; }

    }
}