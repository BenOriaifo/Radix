using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Radix.Core.Models;

namespace Radix.Data
{
    public partial class RadixNotificationContext : DbContext
    {
        public virtual DbSet<AdhocMessage> AdhocMessages { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<MessageTemplate> MessageTemplates { get; set; }
        public virtual DbSet<MessageType> MessageTypes { get; set; }
        public virtual DbSet<NotificationPreferrence> NotificationPreferrences { get; set; }
        public virtual DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }
        public virtual DbSet<ServiceConfiguration> ServiceConfigurations { get; set; }

        public RadixNotificationContext(DbContextOptions<RadixNotificationContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(@"Server=HO-ITAPP348\SQLEXPRESS;Database=radixNotification;Trusted_Connection=true;MultipleActiveResultSets=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdhocMessage>(entity =>
            {
                entity.Property(e => e.DateCreated).HasColumnType("date");

                entity.Property(e => e.EmailMessage).IsUnicode(false);

                entity.Property(e => e.Recipient)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Smsmessage)
                    .HasColumnName("SMSMessage")
                    .HasMaxLength(200)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.Property(e => e.DateSent).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.EmailMessage).IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MessageCode)
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.MessageId)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RSAPIN)
                    .HasColumnName("RSAPIN")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EmployerName)
                    .HasColumnName("EmployerName")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.EmployerCode)
                    .HasColumnName("EmployerCode")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DateExtracted)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SmsMessage)
                    .HasColumnName("SMSMessage")
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<MessageTemplate>(entity =>
            {
                entity.Property(e => e.EmailTemplate).IsUnicode(false);

                entity.Property(e => e.SmsTemplate)
                    .HasColumnName("SMSTemplate")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.MessageTemplate)
                    .HasForeignKey(d => d.MessageTypeId)
                    .HasConstraintName("FK_MessageTemplate_MessageType");
            });

            modelBuilder.Entity<MessageType>(entity =>
            {
                entity.Property(e => e.Code)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Type)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<NotificationPreferrence>(entity =>
            {
                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.NotificationPreferrence)
                    .HasForeignKey(d => d.MessageTypeId)
                    .HasConstraintName("FK_NotificationPreferrence_MessageType");

                entity.HasOne(d => d.NotificationSubscription)
                    .WithMany(p => p.NotificationPreferrences)
                    .HasForeignKey(d => d.NotificationSubscriptionId)
                    .HasConstraintName("FK_NotificationPreferrence_NotificationSubscription");
            });

            modelBuilder.Entity<NotificationSubscription>(entity =>
            {
                entity.Property(e => e.Email)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Pin)
                    .HasColumnName("PIN")
                    .HasColumnType("nchar(10)");
            });

            modelBuilder.Entity<ServiceConfiguration>(entity =>
            {
                entity.HasOne(d => d.MessageType)
                    .WithMany(p => p.ServiceConfiguration)
                    .HasForeignKey(d => d.MessageTypeId)
                    .HasConstraintName("FK_ServiceConfiguration_MessageType");
            });
        }
    }
}
