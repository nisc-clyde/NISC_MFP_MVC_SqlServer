namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_logs_cardreader")]
    public partial class tb_logs_cardreader
    {
        [Key]
        public int serial { get; set; }

        [StringLength(10)]
        public string cr_id { get; set; }

        [StringLength(10)]
        public string card_id { get; set; }

        public int? value { get; set; }

        [StringLength(12)]
        public string seq_no { get; set; }

        [StringLength(20)]
        public string login_type { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? log_time { get; set; }

        public int? mfp_serial { get; set; }
    }
}
