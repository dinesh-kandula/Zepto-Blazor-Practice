using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Data;
using ModelsClassLibrary.Models;
using Zepto_API_Backup.Services;
using Zepto_API_Backup.Context;

namespace Zepto_API_Backup.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ZeptoPrimaryContext _context;

        public ProductRepository(ZeptoPrimaryContext context) : base(context)
        {
            _context = context;
        }

        public override Task<List<Product>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public List<Product> ADOGetAllAsync(String? productName)
        {
            var productParam = new DynamicParameters();
            if (productName != null)
            {
                productParam.Add("@productName", productName);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    using var connection = _context.CreateConnection();
                    {
                        var result = connection.Query<Product>("spProduct_ByName", productParam, commandType: CommandType.StoredProcedure);
                        return result.ToList();
                    }
                }catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            };
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
