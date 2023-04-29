using System;

namespace NISC_MFP_MVC_Service.DTOs.Info.Deposit
{
    public class DepositInfo
    {
        public string user_name { get; set; }
        public string user_id { get; set; }
        public string card_id { get; set; }
        public string card_user_id { get; set; }
        public string card_user_name { get; set; }
        public int? pbalance { get; set; }
        public int? deposit_value { get; set; }
        public int? final_value { get; set; }
        public string deposit_date { get; set; }
    }
}
