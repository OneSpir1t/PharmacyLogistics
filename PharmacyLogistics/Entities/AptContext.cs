using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PharmacyLogistics.Entities
{
    public partial class AptContext : DbContext
    {
        public AptContext()
        {
        }

        public static AptContext aptContext { get; private set; } = new AptContext();
        public AptContext(DbContextOptions<AptContext> options)
            : base(options)
        {
        }

        public virtual DbSet<City> Cities { get; set; } = null!;
        public virtual DbSet<Country> Countries { get; set; } = null!;
        public virtual DbSet<Manufacturer> Manufacturers { get; set; } = null!;
        public virtual DbSet<Pharmacy> Pharmacies { get; set; } = null!;
        public virtual DbSet<Pharmacyproduct> Pharmacyproducts { get; set; } = null!;
        public virtual DbSet<Product> Products { get; set; } = null!;
        public virtual DbSet<Releaseform> Releaseforms { get; set; } = null!;
        public virtual DbSet<Request> Requests { get; set; } = null!;
        public virtual DbSet<Requestproduct> Requestproducts { get; set; } = null!;
        public virtual DbSet<Status> Statuses { get; set; } = null!;
        public virtual DbSet<Supplier> Suppliers { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<Userrole> Userroles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseMySql("server=localhost;user=root;database=db_apteka;password=root", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.UseCollation("utf8mb4_0900_ai_ci")
                .HasCharSet("utf8mb4");

            modelBuilder.Entity<City>(entity =>
            {
                entity.ToTable("city");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.ToTable("country");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Manufacturer>(entity =>
            {
                entity.ToTable("manufacturer");

                entity.HasIndex(e => e.CountryId, "FK_Manufacturer_Country");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Manufacturers)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Manufacturer_Country");
            });

            modelBuilder.Entity<Pharmacy>(entity =>
            {
                entity.ToTable("pharmacy");

                entity.HasIndex(e => e.CityId, "FK_Pharmacy_City");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.CityId).HasColumnName("CityID");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.Pharmacies)
                    .HasForeignKey(d => d.CityId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pharmacy_City");
            });

            modelBuilder.Entity<Pharmacyproduct>(entity =>
            {
                entity.HasKey(e => e.Expirydate)
                    .HasName("PRIMARY");

                entity.ToTable("pharmacyproduct");

                entity.HasIndex(e => e.PharmacyId, "fk_pharmacyproduct_pharmacy_idx");

                entity.HasIndex(e => e.ProductId, "fk_pharmacyproduct_product_idx");

                entity.Property(e => e.Expirydate).HasColumnName("expirydate");

                entity.Property(e => e.PharmacyId).HasColumnName("PharmacyID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Pharmacy)
                    .WithMany(p => p.Pharmacyproducts)
                    .HasForeignKey(d => d.PharmacyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pharmacyproduct_pharmacy");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Pharmacyproducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_pharmacyproduct_product");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.ReleaseFormId, "FK_Product_ReleaseForm");

                entity.HasIndex(e => e.SupplierId, "FK_Product_Supplier");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Article).HasMaxLength(100);

                entity.Property(e => e.Dose).HasMaxLength(120);

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Quantityinthepackage).HasColumnName("quantityinthepackage");

                entity.Property(e => e.ReleaseFormId).HasColumnName("ReleaseFormID");

                entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

                entity.HasOne(d => d.ReleaseForm)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ReleaseFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ReleaseForm");

                entity.HasOne(d => d.Supplier)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SupplierId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Supplier");
            });

            modelBuilder.Entity<Releaseform>(entity =>
            {
                entity.ToTable("releaseform");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("request");

                entity.HasIndex(e => e.PharmacyId, "FK_Request_Pharmacy");

                entity.HasIndex(e => e.StatusId, "FK_Request_Status");

                entity.HasIndex(e => e.UserId, "FK_Request_User");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PharmacyId).HasColumnName("PharmacyID");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Pharmacy)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.PharmacyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_Pharmacy");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Request_Status");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Requests)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Request_User");
            });

            modelBuilder.Entity<Requestproduct>(entity =>
            {
                entity.HasKey(e => new { e.RequestId, e.ProductId })
                    .HasName("PRIMARY")
                    .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

                entity.ToTable("requestproduct");

                entity.HasIndex(e => e.ProductId, "FK_RequestProduct_Product");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Requestproducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestProduct_Product");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.Requestproducts)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestProduct_Request");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("status");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.ToTable("supplier");

                entity.HasIndex(e => e.CountryId, "FK_Supplier_Country");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.Inn).HasColumnName("INN");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.Suppliers)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Supplier_Country");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.PharmacyId, "FK_User_Pharmacy");

                entity.HasIndex(e => e.UserRoleId, "FK_User_UserRole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Login).HasMaxLength(150);

                entity.Property(e => e.Name)
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.Password).HasMaxLength(150);

                entity.Property(e => e.Patryonomic)
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.PharmacyId).HasColumnName("PharmacyID");

                entity.Property(e => e.Phone).HasMaxLength(20);

                entity.Property(e => e.Surname)
                    .HasMaxLength(150)
                    .IsFixedLength();

                entity.Property(e => e.UserRoleId).HasColumnName("UserRoleID");

                entity.HasOne(d => d.Pharmacy)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.PharmacyId)
                    .HasConstraintName("FK_User_Pharmacy");

                entity.HasOne(d => d.UserRole)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.UserRoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserRole");
            });

            modelBuilder.Entity<Userrole>(entity =>
            {
                entity.ToTable("userrole");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
