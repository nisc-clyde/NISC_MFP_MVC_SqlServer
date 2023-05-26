using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace NISC_MFP_MVC_Repository.DB
{
    public partial class MFP_DB : DbContext
    {
        public MFP_DB()
            : base("name=MFPContext")
        {
        }

        public virtual DbSet<doc_detail> doc_detail { get; set; }
        public virtual DbSet<doc_mng> doc_mng { get; set; }
        public virtual DbSet<tb_card> tb_card { get; set; }
        public virtual DbSet<tb_cardreader> tb_cardreader { get; set; }
        public virtual DbSet<tb_department> tb_department { get; set; }
        public virtual DbSet<tb_group> tb_group { get; set; }
        public virtual DbSet<tb_logs_deposit> tb_logs_deposit { get; set; }
        public virtual DbSet<tb_logs_history> tb_logs_history { get; set; }
        public virtual DbSet<tb_logs_remote_history> tb_logs_remote_history { get; set; }
        public virtual DbSet<tb_logs_sentmail> tb_logs_sentmail { get; set; }
        public virtual DbSet<tb_mfp> tb_mfp { get; set; }
        public virtual DbSet<tb_pdfback> tb_pdfback { get; set; }
        public virtual DbSet<tb_scan> tb_scan { get; set; }
        public virtual DbSet<tb_use_page> tb_use_page { get; set; }
        public virtual DbSet<tb_user> tb_user { get; set; }
        public virtual DbSet<tb_watermark> tb_watermark { get; set; }
        public virtual DbSet<tb_deleted_card> tb_deleted_card { get; set; }
        public virtual DbSet<tb_deleted_department> tb_deleted_department { get; set; }
        public virtual DbSet<tb_deleted_user> tb_deleted_user { get; set; }
        public virtual DbSet<tb_logs_cardreader> tb_logs_cardreader { get; set; }
        public virtual DbSet<tb_logs_import> tb_logs_import { get; set; }
        public virtual DbSet<tb_logs_print> tb_logs_print { get; set; }
        public virtual DbSet<tb_print_price> tb_print_price { get; set; }
        public virtual DbSet<tb_token> tb_token { get; set; }
        public virtual DbSet<tblusers> tblusers { get; set; }
        public virtual DbSet<watermark> watermark { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<doc_mng>()
                .Property(e => e.ntime)
                .HasPrecision(0);

            modelBuilder.Entity<doc_mng>()
                .Property(e => e.data_source)
                .IsFixedLength();

            modelBuilder.Entity<doc_mng>()
                .Property(e => e.file_ext)
                .IsFixedLength();

            modelBuilder.Entity<tb_card>()
                .Property(e => e.card_id)
                .IsFixedLength();

            modelBuilder.Entity<tb_card>()
                .Property(e => e.card_type)
                .IsFixedLength();

            modelBuilder.Entity<tb_card>()
                .Property(e => e.enable)
                .IsFixedLength();

            modelBuilder.Entity<tb_card>()
                .Property(e => e.user_id)
                .IsFixedLength();

            modelBuilder.Entity<tb_cardreader>()
                .Property(e => e.cr_id)
                .IsFixedLength();

            modelBuilder.Entity<tb_cardreader>()
                .Property(e => e.cr_ip)
                .IsFixedLength();

            modelBuilder.Entity<tb_cardreader>()
                .Property(e => e.cr_port)
                .IsFixedLength();

            modelBuilder.Entity<tb_cardreader>()
                .Property(e => e.history_date)
                .HasPrecision(0);

            modelBuilder.Entity<tb_cardreader>()
                .Property(e => e.card_update_date)
                .HasPrecision(0);

            modelBuilder.Entity<tb_logs_deposit>()
                .Property(e => e.deposit_date)
                .HasPrecision(0);

            modelBuilder.Entity<tb_logs_history>()
                .Property(e => e.date_time)
                .HasPrecision(0);

            modelBuilder.Entity<tb_logs_remote_history>()
                .Property(e => e.start_date_time)
                .HasPrecision(0);

            modelBuilder.Entity<tb_logs_remote_history>()
                .Property(e => e.end_date_time)
                .HasPrecision(0);

            modelBuilder.Entity<tb_logs_sentmail>()
                .Property(e => e.sent_date)
                .HasPrecision(0);

            modelBuilder.Entity<tb_mfp>()
                .Property(e => e.meter_adjust_time)
                .HasPrecision(0);

            modelBuilder.Entity<tb_pdfback>()
                .Property(e => e.timestamp)
                .IsUnicode(false);

            modelBuilder.Entity<tb_pdfback>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<tb_pdfback>()
                .Property(e => e.jobType)
                .IsUnicode(false);

            modelBuilder.Entity<tb_pdfback>()
                .Property(e => e.serverName)
                .IsUnicode(false);

            modelBuilder.Entity<tb_pdfback>()
                .Property(e => e.serverVersion)
                .IsUnicode(false);

            modelBuilder.Entity<tb_pdfback>()
                .Property(e => e.printerAddr)
                .IsUnicode(false);

            modelBuilder.Entity<tb_pdfback>()
                .Property(e => e.file)
                .IsUnicode(false);

            modelBuilder.Entity<tb_user>()
                .Property(e => e.copy_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_user>()
                .Property(e => e.print_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_user>()
                .Property(e => e.scan_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_user>()
                .Property(e => e.fax_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_card>()
                .Property(e => e.timestamp)
                .HasPrecision(0);

            modelBuilder.Entity<tb_deleted_card>()
                .Property(e => e.card_id)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_card>()
                .Property(e => e.card_type)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_card>()
                .Property(e => e.enable)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_card>()
                .Property(e => e.user_id)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_department>()
                .Property(e => e.timestamp)
                .HasPrecision(0);

            modelBuilder.Entity<tb_deleted_user>()
                .Property(e => e.timestamp)
                .HasPrecision(0);

            modelBuilder.Entity<tb_deleted_user>()
                .Property(e => e.copy_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_user>()
                .Property(e => e.print_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_user>()
                .Property(e => e.scan_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_deleted_user>()
                .Property(e => e.fax_enable_flag)
                .IsFixedLength();

            modelBuilder.Entity<tb_logs_cardreader>()
                .Property(e => e.log_time)
                .HasPrecision(0);

            modelBuilder.Entity<tb_logs_import>()
                .Property(e => e.import_date)
                .HasPrecision(0);

            modelBuilder.Entity<tb_logs_print>()
                .Property(e => e.print_date)
                .HasPrecision(0);

            modelBuilder.Entity<watermark>()
                .Property(e => e.ntime)
                .HasPrecision(0);
        }
    }
}
