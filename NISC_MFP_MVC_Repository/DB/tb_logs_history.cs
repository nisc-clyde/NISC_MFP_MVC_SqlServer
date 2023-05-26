namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_logs_history")]
    public partial class tb_logs_history
    {
        public int id { get; set; }

        [StringLength(50)]
        public string login_user_id { get; set; }

        [StringLength(100)]
        public string login_user_name { get; set; }

        public string operation { get; set; }

        public string affected_data { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? date_time { get; set; }
    }
}
