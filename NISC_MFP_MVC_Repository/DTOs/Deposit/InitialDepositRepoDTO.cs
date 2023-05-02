using System;

namespace NISC_MFP_MVC_Repository.DTOs.Deposit
{
    public class InitialDepositRepoDTO
    {
        private int _deposit_value = 0;
        private int _pbalance = 0;
        private int _final_value = 0;

        public int serial { get; set; }
        public string user_id { get; set; } = null;
        public string user_name { get; set; } = null;
        public string card_id { get; set; } = null;
        public string card_user_id { get; set; } = null;
        public string card_user_name { get; set; } = null;
        public int? deposit_value { get { return _deposit_value; } set { _deposit_value = value ?? 0; } }
        public int? pbalance { get { return _pbalance; } set { _pbalance = value ?? 0; } }
        public int? final_value { get { return _final_value; } set { _final_value = value ?? 0; } }
        public DateTime? deposit_date { get; set; } = null;
        public string group_id { get; set; } = null;
    }
}
