namespace NISC_MFP_MVC_Repository.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_print_price")]
    public partial class tb_print_price
    {
        [Key]
        public int serial { get; set; }

        [StringLength(10)]
        public string page_size { get; set; }

        [StringLength(1)]
        public string color { get; set; }

        public int? price { get; set; }
    }
}
