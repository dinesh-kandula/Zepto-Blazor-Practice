using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models
{
    public class ZeptoContext : DbContext
    {
        public ZeptoContext(DbContextOptions<ZeptoContext> options) : base(options)
        {
        }

        public virtual DbSet<Product> Products { get; set; }

        public virtual DbSet<PizzaSpecial> PizzaSpecials { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
               .Entity<Product>()
               .Property(e => e.Category)
               .HasConversion<string>();
        }
    }
}
