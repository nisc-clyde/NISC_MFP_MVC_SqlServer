using System;

namespace NISC_MFP_MVC_Repository.DTOs.DocumentManagement
{
    //The print jobs model
    public class doc_mng
    {
        public string doc_uid { get; set; }
        public DateTime? ntime { get; set; } = null;
        public string doc_name { get; set; } = null;
        public int? page_count { get; set; } = null;
        public string page_size { get; set; } = null;
        public int? bc_print { get; set; } = null;
    }
}
