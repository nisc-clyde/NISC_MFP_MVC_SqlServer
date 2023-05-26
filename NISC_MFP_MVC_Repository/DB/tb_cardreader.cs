namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_cardreader")]
    public partial class tb_cardreader
    {
        [Key]
        public int serial { get; set; }

        [Required]
        [StringLength(10)]
        public string cr_id { get; set; }

        [StringLength(15)]
        public string cr_ip { get; set; }

        [Required]
        [StringLength(5)]
        public string cr_port { get; set; }

        [Required]
        [StringLength(10)]
        public string cr_type { get; set; }

        [Required]
        [StringLength(10)]
        public string cr_mode { get; set; }

        [StringLength(10)]
        public string cr_card_switch { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? history_date { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? card_update_date { get; set; }

        [StringLength(20)]
        public string cr_status { get; set; }

        [StringLength(20)]
        public string cr_version { get; set; }

        [StringLength(10)]
        public string cr_relay_status { get; set; }
    }
}
