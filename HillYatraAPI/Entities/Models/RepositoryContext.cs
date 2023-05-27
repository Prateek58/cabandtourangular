using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace HillYatraAPI.Models
{
    public partial class RepositoryContext : DbContext
    {
        public RepositoryContext()
        {
        }

        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Booking> Booking { get; set; }
        public virtual DbSet<ContactUs> ContactUs { get; set; }
        public virtual DbSet<Efmigrationshistory> Efmigrationshistory { get; set; }
        public virtual DbSet<LkpCategory> LkpCategory { get; set; }
        public virtual DbSet<LkpType> LkpType { get; set; }
        public virtual DbSet<LkpUsertype> LkpUsertype { get; set; }
        public virtual DbSet<Places> Places { get; set; }
        public virtual DbSet<Shedule> Shedule { get; set; }
        public virtual DbSet<SheduleImages> SheduleImages { get; set; }
        public virtual DbSet<Sysdiagrams> Sysdiagrams { get; set; }
        public virtual DbSet<Transport> Transport { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseMySQL("server=140.238.163.175;port=3306;user=boyotrav_admin;password=HillYat@#135;database=boyotravDB_test");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>(entity =>
            {
                entity.ToTable("booking");

                entity.HasIndex(e => e.PassengerId)
                    .HasName("FK_Booking_Passenger_idx");

                entity.HasIndex(e => e.SheduleId)
                    .HasName("FK_Booking_Shedule_idx");

                entity.HasIndex(e => e.VenderId)
                    .HasName("FK_Booking_Vendor_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.PassengerId).HasColumnName("PassengerID");

                entity.Property(e => e.SheduleId).HasColumnName("SheduleID");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.VenderId).HasColumnName("VenderID");

                entity.HasOne(d => d.Passenger)
                    .WithMany(p => p.BookingPassenger)
                    .HasForeignKey(d => d.PassengerId)
                    .HasConstraintName("FK_Booking_Passenger");

                entity.HasOne(d => d.Shedule)
                    .WithMany(p => p.Booking)
                    .HasForeignKey(d => d.SheduleId)
                    .HasConstraintName("FK_Booking_Shedule");

                entity.HasOne(d => d.Vender)
                    .WithMany(p => p.BookingVender)
                    .HasForeignKey(d => d.VenderId)
                    .HasConstraintName("FK_Booking_Vendor");
            });

            modelBuilder.Entity<ContactUs>(entity =>
            {
                entity.ToTable("contactUs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CabModel)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.CabNumber)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Comments)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Efmigrationshistory>(entity =>
            {
                entity.HasKey(e => e.MigrationId)
                    .HasName("PRIMARY");

                entity.ToTable("__efmigrationshistory");

                entity.Property(e => e.MigrationId)
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ProductVersion)
                    .IsRequired()
                    .HasMaxLength(32)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LkpCategory>(entity =>
            {
                entity.ToTable("lkp_category");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Category)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<LkpType>(entity =>
            {
                entity.ToTable("lkp_type");

                entity.HasIndex(e => e.Categoy)
                    .HasName("fk_type_category_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Categoy).HasColumnName("categoy");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.HasOne(d => d.CategoyNavigation)
                    .WithMany(p => p.LkpType)
                    .HasForeignKey(d => d.Categoy)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_type_category");
            });

            modelBuilder.Entity<LkpUsertype>(entity =>
            {
                entity.ToTable("lkp_usertype");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            modelBuilder.Entity<Places>(entity =>
            {
                entity.ToTable("places");

                entity.HasIndex(e => e.Type)
                    .HasName("FK-type_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Place)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Places)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK-type");
            });

            modelBuilder.Entity<Shedule>(entity =>
            {
                entity.ToTable("shedule");

                entity.HasIndex(e => e.FromPlace)
                    .HasName("FK_Shedule_PlacesFrom_idx");

                entity.HasIndex(e => e.ToPlace)
                    .HasName("FK_Shedule_PlacesTo_idx");

                entity.HasIndex(e => e.TransportId)
                    .HasName("FK_Shedule_Transport_idx");

                entity.HasIndex(e => e.Type)
                    .HasName("fk_shedule_lkp_type_idx");

                entity.HasIndex(e => e.VendorId)
                    .HasName("FK_Shedule_Vendor_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.DateTimePickup).HasColumnType("datetime(6)");

                entity.Property(e => e.DateTimeReturn).HasColumnType("datetime(6)");

                entity.Property(e => e.Description)
                    .HasMaxLength(5000)
                    .IsUnicode(false);

                entity.Property(e => e.DescriptionTitle)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FuelType)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.IsFeatured).HasDefaultValueSql("'0'");

                entity.Property(e => e.TransportId).HasColumnName("TransportID");

                entity.Property(e => e.TripType)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.HasOne(d => d.FromPlaceNavigation)
                    .WithMany(p => p.SheduleFromPlaceNavigation)
                    .HasForeignKey(d => d.FromPlace)
                    .HasConstraintName("FK_Shedule_PlacesFrom");

                entity.HasOne(d => d.ToPlaceNavigation)
                    .WithMany(p => p.SheduleToPlaceNavigation)
                    .HasForeignKey(d => d.ToPlace)
                    .HasConstraintName("FK_Shedule_PlacesTo");

                entity.HasOne(d => d.Transport)
                    .WithMany(p => p.Shedule)
                    .HasForeignKey(d => d.TransportId)
                    .HasConstraintName("FK_Shedule_Transport");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Shedule)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shedule_lkp_type");

                entity.HasOne(d => d.Vendor)
                    .WithMany(p => p.Shedule)
                    .HasForeignKey(d => d.VendorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shedule_Vendor");
            });

            modelBuilder.Entity<SheduleImages>(entity =>
            {
                entity.ToTable("sheduleImages");

                entity.HasIndex(e => e.SheduleId)
                    .HasName("FK_SheduleImage_Shedule_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.ImageSrc)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.SheduleId).HasColumnName("SheduleID");

                entity.HasOne(d => d.Shedule)
                    .WithMany(p => p.SheduleImages)
                    .HasForeignKey(d => d.SheduleId)
                    .HasConstraintName("FK_SheduleImage_Shedule");
            });

            modelBuilder.Entity<Sysdiagrams>(entity =>
            {
                entity.HasKey(e => e.DiagramId)
                    .HasName("PRIMARY");

                entity.ToTable("sysdiagrams");

                entity.HasIndex(e => new { e.PrincipalId, e.Name })
                    .HasName("UK_principal_name")
                    .IsUnique();

                entity.Property(e => e.DiagramId).HasColumnName("diagram_id");

                entity.Property(e => e.Definition)
                    .HasColumnName("definition")
                    .HasColumnType("longblob");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(160)
                    .IsUnicode(false);

                entity.Property(e => e.PrincipalId).HasColumnName("principal_id");

                entity.Property(e => e.Version).HasColumnName("version");
            });

            modelBuilder.Entity<Transport>(entity =>
            {
                entity.ToTable("transport");

                entity.HasIndex(e => e.TransportType)
                    .HasName("fk_transport_type_idx");

                entity.HasIndex(e => e.VenderId)
                    .HasName("FK_Transport_Vendor_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Description)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Image)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Model)
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Title)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.VenderId).HasColumnName("VenderID");

                entity.HasOne(d => d.TransportTypeNavigation)
                    .WithMany(p => p.Transport)
                    .HasForeignKey(d => d.TransportType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_transport_type");

                entity.HasOne(d => d.Vender)
                    .WithMany(p => p.Transport)
                    .HasForeignKey(d => d.VenderId)
                    .HasConstraintName("FK_Transport_Vendor");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.UserTypeId)
                    .HasName("a_idx");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Email)
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Gender)
                    .HasMaxLength(1)
                    .IsFixedLength();

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasDefaultValueSql("b'1'");

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Mobile)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Photo).HasColumnType("longblob");

                entity.Property(e => e.UpdatedOn).HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.UserName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserTypeId)
                    .HasConstraintName("FK_User_UserType");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
