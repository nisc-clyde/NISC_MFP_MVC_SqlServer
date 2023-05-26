namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_pdfback")]
    public partial class tb_pdfback
    {
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string timestamp { get; set; }

        [Required]
        [StringLength(50)]
        public string username { get; set; }

        [Required]
        [StringLength(50)]
        public string jobType { get; set; }

        [Required]
        [StringLength(50)]
        public string serverName { get; set; }

        [Required]
        [StringLength(50)]
        public string serverVersion { get; set; }

        [Required]
        [StringLength(50)]
        public string printerAddr { get; set; }

        [Required]
        [StringLength(50)]
        public string file { get; set; }
    }
}
