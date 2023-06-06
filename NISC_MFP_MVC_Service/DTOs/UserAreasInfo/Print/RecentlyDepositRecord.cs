using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print
{
    public class RecentlyDepositRecord
    {
        public string user_id { get; set; }
        public string user_name { get; set; }
        public int? pbalance { get; set; }
        public int? deposit_value { get; set; }
        public int? final_value { get; set; }
        public string deposit_date { get; set; }
        
    }
}
