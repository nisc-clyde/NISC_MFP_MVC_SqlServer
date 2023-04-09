using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    public class DataTableRequest
    {
        public DataTableRequest(NameValueCollection form)
        {
            SearchDTO = new List<AbstractSearchDTO>();
            Draw = Convert.ToInt32(form["draw"]);
            Start = Convert.ToInt32(form["start"]);
            Length = Convert.ToInt32(form["length"]);
            GlobalSearchValue = form["search[value]"];
            SortColumnIndex = form["order[0][column]"];
            SortColumnName = form["columns[" + form["order[0][column]"] + "][name]"];
            SortDirection = form["order[0][dir]"];

            //Filter data by column
            ColumnSearchPrint_Printer = form["columns[0][search][value]"];
            ColumnSearchPrint_User = form["columns[1][search][value]"];
            ColumnSearchPrint_Department = form["columns[2][search][value]"];
            ColumnSearchPrint_Card = form["columns[3][search][value]"];
            ColumnSearchPrint_AttributeSelect = form["columns[4][search][value]"];
            ColumnSearchPrint_ActionSelect = form["columns[5][search][value]"];
            ColumnSearchPrint_ColorSelect = form["columns[6][search][value]"];
            ColumnSearchPrint_Count = form["columns[7][search][value]"];
            ColumnSearchPrint_Point = form["columns[8][search][value]"];
            ColumnSearchPrint_PrintTime = form["columns[9][search][value]"];
            ColumnSearchPrint_DocumentName = form["columns[10][search][value]"];
        }

        public List<AbstractSearchDTO> SearchDTO { get; set; }

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
                switch (this.SortColumnIndex)
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

        public string SortDirection { get; set; }

        public int RecordsTotalGet { get; set; }

        public int RecordsFilteredGet { get; set; }

        public string ColumnSearchPrint_Printer { get; set; }
        public string ColumnSearchPrint_User { get; set; }
        public string ColumnSearchPrint_Department { get; set; }
        public string ColumnSearchPrint_Card { get; set; }
        public string ColumnSearchPrint_AttributeSelect { get; set; }
        public string ColumnSearchPrint_ActionSelect { get; set; }
        public string ColumnSearchPrint_ColorSelect { get; set; }
        public string ColumnSearchPrint_Count { get; set; }
        public string ColumnSearchPrint_Point { get; set; }
        public string ColumnSearchPrint_PrintTime { get; set; }
        public string ColumnSearchPrint_DocumentName { get; set; }

    }
}