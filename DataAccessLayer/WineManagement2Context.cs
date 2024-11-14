using System;
using System.Collections.Generic;
using BusinessObjects.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BusinessObjects;

public partial class WineManagement2Context : DbContext
{
    public WineManagement2Context()
    {
    }

    public WineManagement2Context(DbContextOptions<WineManagement2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Request> Requests { get; set; }

    public virtual DbSet<RequestDetail> RequestDetails { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<WareHouse> WareHouses { get; set; }

    public virtual DbSet<WarehouseWine> WarehouseWines { get; set; }

    public virtual DbSet<Wine> Wines { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer(GetConnectionString());//"Server=SANGNT20072004;Database=WineManagement_2; Uid=sa; Pwd=1234567890;TrustServerCertificate=true");

    private string GetConnectionString()
    {
        IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        return configuration["ConnectionStrings:DB"];
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5866DA8769C");

            entity.ToTable("Account");

            entity.HasIndex(e => e.Username, "UQ__Account__536C85E4CA852973").IsUnique();

            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role).HasMaxLength(10);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2B2DFB7C78");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);
        });

        modelBuilder.Entity<Request>(entity =>
        {
            entity.HasKey(e => e.RequestId).HasName("PK__Request__33A8519A53C564E9");

            entity.ToTable("Request");

            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Account).WithMany(p => p.Requests)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK__Request__Account__4316F928");
        });

        modelBuilder.Entity<RequestDetail>(entity =>
        {
            entity.HasKey(e => e.RequestDetailId).HasName("PK__RequestD__DC528B70C9A6FB81");

            entity.ToTable("RequestDetail");

            entity.Property(e => e.RequestDetailId).HasColumnName("RequestDetailID");
            entity.Property(e => e.RequestId).HasColumnName("RequestID");
            entity.Property(e => e.WineId).HasColumnName("WineID");

            entity.HasOne(d => d.Request).WithMany(p => p.RequestDetails)
                .HasForeignKey(d => d.RequestId)
                .HasConstraintName("FK__RequestDe__Reque__4316F928");

            entity.HasOne(d => d.Wine).WithMany(p => p.RequestDetails)
                .HasForeignKey(d => d.WineId)
                .HasConstraintName("FK__RequestDe__WineI__20C1E124");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__Supplier__4BE666945DD01692");

            entity.ToTable("Supplier");

            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.ContactPerson).HasMaxLength(100);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(15);
            entity.Property(e => e.Status).HasMaxLength(100);
        });

        modelBuilder.Entity<WareHouse>(entity =>
        {
            entity.HasKey(e => e.WareHouseId).HasName("PK__WareHous__69FF8098C59E9395");

            entity.ToTable("WareHouse");

            entity.Property(e => e.WareHouseId).HasColumnName("WareHouseID");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ContactPerson)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Location)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WarehouseWine>(entity =>
        {
            entity.HasKey(e => e.WarehouseWineId).HasName("PK__Warehous__099817CDC48B82A3");

            entity.ToTable("WarehouseWine");

            entity.Property(e => e.WarehouseWineId).HasColumnName("WarehouseWineID");
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.WareHouseId).HasColumnName("WareHouseID");
            entity.Property(e => e.WineId).HasColumnName("WineID");

            entity.HasOne(d => d.WareHouse).WithMany(p => p.WarehouseWines)
                .HasForeignKey(d => d.WareHouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__WareH__71D1E811");

            entity.HasOne(d => d.Wine).WithMany(p => p.WarehouseWines)
                .HasForeignKey(d => d.WineId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__WineI__22AA2996");
        });

        modelBuilder.Entity<Wine>(entity =>
        {
            entity.HasKey(e => e.WineId).HasName("PK__Wine__ABB24B3178FC1D24");

            entity.ToTable("Wine");

            entity.Property(e => e.WineId).HasColumnName("WineID");
            entity.Property(e => e.AlcoholContent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.SupplierId).HasColumnName("SupplierID");

            entity.HasOne(d => d.Category).WithMany(p => p.Wines)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Wine__CategoryID__239E4DCF");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Wines)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK__Wine__SupplierID__24927208");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
