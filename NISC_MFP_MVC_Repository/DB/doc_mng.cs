namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.doc_mng")]
    public partial class doc_mng
    {
        [Key]
        [StringLength(50)]
        public string doc_uid { get; set; }

        [StringLength(50)]
        public string user_id { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ntime { get; set; }

        [StringLength(20)]
        public string doc_name { get; set; }

        [StringLength(20)]
        public string doc_ext { get; set; }

        [StringLength(50)]
        public string page_size { get; set; }

        public int? page_count { get; set; }

        public int? is_user_del { get; set; }

        public int? detail_count { get; set; }

        public int? pkg_per { get; set; }

        public int? duplexing { get; set; }

        public int? inputbin { get; set; }

        public int? copycount { get; set; }

        public int? pageorientation { get; set; }

        public int? is_print { get; set; }

        public int? bc_print { get; set; }

        [Required]
        [StringLength(1)]
        public string data_source { get; set; }

        [Required]
        [StringLength(5)]
        public string file_ext { get; set; }

        [Required]
        [StringLength(50)]
        public string prn_device { get; set; }

        public int is_upload { get; set; }

        [Required]
        [StringLength(50)]
        public string loc_no { get; set; }
    }
}
