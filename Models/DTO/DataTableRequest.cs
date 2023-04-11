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
                switch (PostPageFrom)
                {

                    case "print": return SearchPrintDTO.ColumnName2Property(SortColumnIndex);
                    case "deposite": return SearchDepositeDTO.ColumnName2Property(SortColumnIndex);
                    case "department": return SearchDepartmentDTO.ColumnName2Property(SortColumnIndex);
                    case "user": return SearchUserDTO.ColumnName2Property(SortColumnIndex);
                    case "cardreader": return SearchCardReaderDTO.ColumnName2Property(SortColumnIndex);
                    case "card": return SearchCardDTO.ColumnName2Property(SortColumnIndex);
                    case "watermark": return SearchWatermarkDTO.ColumnName2Property(SortColumnIndex);
                    case "history": return SearchHistoryDTO.ColumnName2Property(SortColumnIndex);
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