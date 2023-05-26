namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.doc_detail")]
    public partial class doc_detail
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(50)]
        public string doc_uid { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int iseq { get; set; }

        public int? page_nos { get; set; }

        public int? page_noe { get; set; }

        public byte[] doc { get; set; }

        public int? is_print { get; set; }
    }
}
