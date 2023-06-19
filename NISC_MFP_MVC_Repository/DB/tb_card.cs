namespace NISC_MFP_MVC_Repository.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_card")]
    public partial class tb_card
    {
        [Key]
        public int serial { get; set; }

        [Required]
        [StringLength(10)]
        public string card_id { get; set; }

        [StringLength(4)]
        public string card_password { get; set; }

        [Required]
        [StringLength(1)]
        public string card_type { get; set; }

        [StringLength(1)]
        public string card_type2 { get; set; }

        public int? value { get; set; }

        public int freevalue { get; set; }

        [StringLength(1)]
        public string enable { get; set; }

        [StringLength(50)]
        public string user_id { get; set; }

        [Required]
        [StringLength(1)]
        public string import_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string if_deleted { get; set; }
    }
}
