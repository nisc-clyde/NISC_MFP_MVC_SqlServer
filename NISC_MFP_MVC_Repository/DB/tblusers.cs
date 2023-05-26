namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tblusers")]
    public partial class tblusers
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(50)]
        public string FULLNAME { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string EMAIL { get; set; }

        [Key]
        [Column(Order = 3)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IS_ACTIVE { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(50)]
        public string PWD { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(50)]
        public string LOGIN_TOKEN { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(50)]
        public string SCANSTORAGE { get; set; }

        [Key]
        [Column(Order = 7)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CREDIT { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(50)]
        public string CODE { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(50)]
        public string SYNCSOURCE { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(50)]
        public string LANG { get; set; }

        [Key]
        [Column(Order = 11)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int SESSIONREFCNT { get; set; }

        [Key]
        [Column(Order = 12)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int QUOTAACTIVE { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(50)]
        public string PHONE { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(50)]
        public string NOTES { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(50)]
        public string AUTHSERVER { get; set; }

        [Key]
        [Column(Order = 16)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int USEAUTHSERVER { get; set; }
    }
}
