namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_logs_sentmail")]
    public partial class tb_logs_sentmail
    {
        [Key]
        public int serial { get; set; }

        [StringLength(50)]
        public string dept_id { get; set; }

        [StringLength(100)]
        public string dept_name { get; set; }

        public string dest_mail { get; set; }

        public string message { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime sent_date { get; set; }
    }
}
