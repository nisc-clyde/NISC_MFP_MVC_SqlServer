namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_mfp")]
    public partial class tb_mfp
    {
        [Key]
        public int serial { get; set; }

        [StringLength(10)]
        public string cr_id { get; set; }

        [Required]
        [StringLength(10)]
        public string printer_id { get; set; }

        [Required]
        [StringLength(10)]
        public string driver_number { get; set; }

        [StringLength(15)]
        public string mfp_ip { get; set; }

        [StringLength(100)]
        public string mfp_name { get; set; }

        [StringLength(50)]
        public string mfp_jt_name { get; set; }

        [Required]
        [StringLength(10)]
        public string mfp_color { get; set; }

        [Required]
        [StringLength(20)]
        public string mfp_status { get; set; }

        public int meter_num { get; set; }

        public int? meter_current { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? meter_adjust_time { get; set; }

        [StringLength(10)]
        public string login_card_id { get; set; }
    }
}
