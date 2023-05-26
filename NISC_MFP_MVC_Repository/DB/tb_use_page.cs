namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_use_page")]
    public partial class tb_use_page
    {
        public int id { get; set; }

        [Required]
        public string file_name { get; set; }

        [Required]
        public string description { get; set; }
    }
}
