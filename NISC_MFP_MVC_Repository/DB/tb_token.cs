namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_token")]
    public partial class tb_token
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string token { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ctime { get; set; }
    }
}
