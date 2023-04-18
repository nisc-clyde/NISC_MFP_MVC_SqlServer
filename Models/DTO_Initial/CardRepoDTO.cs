using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO_Initial
{
    public class CardRepoDTO
    {
        public int serial { get; set; }
        public string card_id { get; set; }
        public string card_password { get; set; }
        public string card_type { get; set; }
        public string card_type2 { get; set; }
        public int value { get; set; }
        public int? freevalue { get; set; }
        public string enable { get; set; }
        public string user_id { get; set; } = "";
        public string import_flag { get; set; } = "0";
        public string if_deleted { get; set; } = "0";
    }
}