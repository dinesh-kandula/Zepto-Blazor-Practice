using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsClassLibrary.Models.Enums;
using System.Data.Common;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ModelsClassLibrary.Models
{
    public class ZeptoContext : DbContext
    {

        private readonly string ConnectionString = "data source=DESKTOP-1GRKTRJ\\SQLEXPRESS;initial catalog=ZeptoDB;integrated security=True;TrustServerCertificate=True";

        public ZeptoContext(DbContextOptions<ZeptoContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<PizzaSpecial> PizzaSpecials { get; set; }

        public virtual ZeptoUser? ZeptoUsers { get; set; }

        public virtual Cart Carts { get; set; }

        public virtual CartItem CartItems { get; set; }

        public IDbConnection CreateConnection()
          => new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=C:\\USERS\\DINESH.KANDULA\\DOCUMENTS\\ZEPTO.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ZeptoUser>()
                .Property(e => e.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(e => e.Category)
                .HasConversion<string>();

            modelBuilder.Entity<ZeptoUser>()
               .Property(e => e.UserType)
               .HasConversion<string>();

            modelBuilder.Entity<ZeptoUser>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<ZeptoUser>()
                .HasIndex(e => e.UserName)
                .IsUnique();

            modelBuilder.Entity<ZeptoUser>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.ZeptoUser)
                .HasForeignKey<Cart>(c => c.ZeptoUserId);
        }
    }
}
