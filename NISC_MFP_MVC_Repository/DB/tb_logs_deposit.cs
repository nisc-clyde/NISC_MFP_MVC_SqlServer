namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_logs_deposit")]
    public partial class tb_logs_deposit
    {
        [Key]
        public int serial { get; set; }

        [StringLength(50)]
        public string user_id { get; set; }

        [StringLength(100)]
        public string user_name { get; set; }

        [StringLength(10)]
        public string card_id { get; set; }

        [StringLength(50)]
        public string card_user_id { get; set; }

        [StringLength(100)]
        public string card_user_name { get; set; }

        public int? deposit_value { get; set; }

        public int? pbalance { get; set; }

        public int? final_value { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? deposit_date { get; set; }

        [StringLength(50)]
        public string group_id { get; set; }
    }
}
