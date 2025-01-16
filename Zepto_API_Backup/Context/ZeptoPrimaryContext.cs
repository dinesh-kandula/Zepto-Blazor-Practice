using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using ModelsClassLibrary.Models;

namespace Zepto_API_Backup.Context;

public partial class ZeptoPrimaryContext : DbContext
{

    public ZeptoPrimaryContext()
    {
    }

    public ZeptoPrimaryContext(DbContextOptions<ZeptoPrimaryContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<PizzaSpecial> PizzaSpecials { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ZeptoUser> ZeptoUsers { get; set; }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
    //        => optionsBuilder.UseSqlServer("Data Source=(localdb)\\NewInstance;Initial Catalog=Zepto_Primary;Integrated Security=True;Trust Server Certificate=True");

    public IDbConnection CreateConnection()
    {
        return new SqlConnection("Data Source=(localdb)\\NewInstance;Initial Catalog=Zepto_Primary;Integrated Security=True;Trust Server Certificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.ToTable("Cart");

            entity.HasIndex(e => e.ZeptoUserId, "IX_Cart_ZeptoUserId").IsUnique();

            entity.HasOne(d => d.ZeptoUser).WithOne(p => p.Cart).HasForeignKey<Cart>(d => d.ZeptoUserId);

            entity.HasMany(d => d.Products).WithMany(p => p.Carts)
                .UsingEntity<Dictionary<string, object>>(
                    "CartProduct",
                    r => r.HasOne<Product>().WithMany().HasForeignKey("ProductsId"),
                    l => l.HasOne<Cart>().WithMany().HasForeignKey("CartsId"),
                    j =>
                    {
                        j.HasKey("CartsId", "ProductsId");
                        j.ToTable("CartProduct");
                        j.HasIndex(new[] { "ProductsId" }, "IX_CartProduct_ProductsId");
                    });
        });

        modelBuilder.Entity<PizzaSpecial>(entity =>
        {
            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.Property(e => e.BasePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Offer).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasDefaultValue("");
        });

        modelBuilder.Entity<ZeptoUser>(entity =>
        {
            entity.ToTable("ZeptoUser");

            entity.HasIndex(e => e.Email, "IX_ZeptoUser_Email").IsUnique();

            entity.HasIndex(e => e.UserName, "IX_ZeptoUser_UserName").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
