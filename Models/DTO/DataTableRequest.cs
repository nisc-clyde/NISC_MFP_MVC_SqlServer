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
            SearchValue = form["search[value]"];
            SortColumnName = form["columns[" + form["order[0][column]"] + "][name]"];
            SortDirection = form["order[0][dir]"];
        }

        public List<AbstractSearchDTO> SearchDTO { get; set; }

        public int Draw { get; set; }

        public int Start { get; set; }

        public int Length { get; set; }

        public string SearchValue { get; set; }


        public string SortColumnName { get; set; }

        public string SortDirection { get; set; }

        public int RecordsTotalGet { get; set; }

        public int RecordsFilteredGet { get; set; }

    }
}