namespace NISC_MFP_MVC_Repository.DTOs.Card
{
    public class InitialCardRepoDTO
    {
        private int? _value;

        public int serial { get; set; }
        public string card_id { get; set; }
        public string card_password { get; set; } = "0000";
        public string card_type { get; set; } = "0";
        public string card_type2 { get; set; } = "0";
        public int? value { get { return _value; } set { _value = value ?? 0; } }
        public int freevalue { get; set; }
        public string enable { get; set; } = "1";
        public string user_id { get; set; } = null;
        public string user_name { get; set; } = "";
        public string import_flag { get; set; } = "0";
        public string if_deleted { get; set; } = "0";
    }
}
