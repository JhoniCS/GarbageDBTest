using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using GarbageDBTest.Domain.Entities;

#nullable disable


namespace GarbageDBTest.Infraestructure.Data
{
    public partial class GarbageDBContext : DbContext
    {
        public GarbageDBContext()
        {
        }

        public GarbageDBContext(DbContextOptions<GarbageDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Poi> Pois { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("name=GarbageDB");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Colony)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("colony");

                entity.Property(e => e.Reason)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("reason");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("date")
                    .HasColumnName("createDate");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.Features)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("features");

                entity.Property(e => e.Geoubication).HasColumnName("geoubication");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("name");

                entity.Property(e => e.RequiredPersons).HasColumnName("requiredPersons");

                entity.Property(e => e.Specialconditions)
                    .HasMaxLength(300)
                    .IsUnicode(false)
                    .HasColumnName("specialconditions");

                entity.Property(e => e.Sponsor)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("sponsor");

                entity.Property(e => e.Time)
                    .HasColumnType("time(4)")
                    .HasColumnName("time");
            });

            modelBuilder.Entity<Poi>(entity =>
            {
                entity.ToTable("POIs");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Colony)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("colony");

                entity.Property(e => e.Confirmations).HasColumnName("confirmations");

                entity.Property(e => e.Negations).HasColumnName("negations");

                entity.Property(e => e.Date)
                    .HasColumnType("datetime")
                    .HasColumnName("date");

                entity.Property(e => e.Description)
                    .HasMaxLength(200)
                    .IsUnicode(false)
                    .HasColumnName("description");

                entity.Property(e => e.Geoubication).HasColumnName("geoubication");

                entity.Property(e => e.IsDeleted).HasColumnName("isDeleted");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnType("datetime")
                    .HasColumnName("modifiedDate");

                entity.Property(e => e.Photo)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("photo");

                entity.Property(e => e.Reason)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("reason");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
