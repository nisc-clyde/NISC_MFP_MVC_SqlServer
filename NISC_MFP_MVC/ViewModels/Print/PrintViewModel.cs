namespace NISC_MFP_MVC.ViewModels
{
    public class PrintViewModel : AbstractViewModel
    {
        public string mfp_name { get; set; }

        public string user_name { get; set; }

        public string dept_name { get; set; }

        public string card_id { get; set; }

        public string card_type { get; set; }

        public string usage_type { get; set; }

        public string page_color { get; set; }

        public int? page { get; set; }

        public int? value { get; set; }

        public string print_date { get; set; }

        public string document_name { get; set; }

        public int serial { get; set; }
    }
}