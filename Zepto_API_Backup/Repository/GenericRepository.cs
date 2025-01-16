using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Zepto_API_Backup.Services;
using Zepto_API_Backup.Context;

namespace Zepto_API_Backup.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ZeptoPrimaryContext dbContext;

        internal DbSet<T> DbSet { get; set; }

        public GenericRepository(ZeptoPrimaryContext context)
        {
            this.dbContext = context;
            this.DbSet = this.dbContext.Set<T>();
        }

        public virtual Task<List<T>> GetAllAsync()
        {
            return this.DbSet.ToListAsync();
        }

        public virtual Task<T> AddEntity(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> DeleteEntity(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public virtual Task<bool> UpdateEntity(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
