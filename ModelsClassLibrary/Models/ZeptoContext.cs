using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsClassLibrary.Models.Enums;

namespace ModelsClassLibrary.Models
{
    public class ZeptoContext : DbContext
    {
        public ZeptoContext(DbContextOptions<ZeptoContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<PizzaSpecial> PizzaSpecials { get; set; }

        public virtual User Users { get; set; }

        public virtual Cart Carts { get; set; }

        public virtual CartItem CartItems { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(e => e.Gender)
                .HasConversion<string>();

            modelBuilder.Entity<Product>()
                .Property(e => e.Category)
                .HasConversion<string>();

            modelBuilder.Entity<User>()
               .Property(e => e.UserType)
               .HasConversion<string>();

            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(e => e.UserName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(c => c.User)
                .HasForeignKey<Cart>(c => c.UserId);
        }
    }
}
