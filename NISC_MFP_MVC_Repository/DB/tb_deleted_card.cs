namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_deleted_card")]
    public partial class tb_deleted_card
    {
        [Key]
        [Column(TypeName = "datetime2")]
        public DateTime timestamp { get; set; }

        public int? serial { get; set; }

        [StringLength(10)]
        public string card_id { get; set; }

        [StringLength(4)]
        public string card_password { get; set; }

        [StringLength(1)]
        public string card_type { get; set; }

        [StringLength(1)]
        public string card_type2 { get; set; }

        public int? value { get; set; }

        public int? freevalue { get; set; }

        [StringLength(1)]
        public string enable { get; set; }

        [StringLength(50)]
        public string user_id { get; set; }

        [StringLength(1)]
        public string import_flag { get; set; }

        [StringLength(1)]
        public string if_deleted { get; set; }
    }
}
