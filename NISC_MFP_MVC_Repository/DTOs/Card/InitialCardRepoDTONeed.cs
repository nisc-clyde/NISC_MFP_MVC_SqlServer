namespace NISC_MFP_MVC_Repository.DTOs.Card
{
    public class InitialCardRepoDTONeed
    {
        private int? _value;

        public string card_id { get; set; }
        public int? value { get { return _value; } set { _value = value ?? 0; } }
        public int freevalue { get; set; }
        public string user_id { get; set; } = null;
        public string user_name { get; set; } = "";
        public string card_type { get; set; } = null;
        public string enable { get; set; } = "1";
        public int serial { get; set; }
    }
}
