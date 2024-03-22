using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using ModelsClassLibrary.Models;
using Microsoft.Data.SqlClient;

namespace ModelsClassLibrary.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ZeptoContext _context;

        public ProductRepository(ZeptoContext context) : base(context)
        {
            _context = context;
        }

        public override Task<List<Product>> GetAllAsync()
        {
            return base.GetAllAsync();
        }


        public List<Product> GetAllProductsWithFilter(SqlParameter? productParam)
        {
            List<Product> result;

            if (productParam != null)
            {
                result = [.. _context.Products.FromSqlRaw("EXEC dbo.spProduct_ByName @productName", productParam),];
            }
            else
            {
                result = [.. _context.Products.FromSqlRaw("EXEC dbo.spProduct_ByName")];
            }

            return result;
        }



        public override async Task<Product> GetAsync(int id)
        {
            return await DbSet.FirstOrDefaultAsync(item => item.Id == id);
        }

        public override async Task<Product> AddEntity(Product entity)
        {
            var result = await DbSet.AddAsync(entity);
            return result.Entity;
        }
    }
}
