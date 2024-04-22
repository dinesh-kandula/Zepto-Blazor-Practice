using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Services
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetAsync(int id);
        Task<T> AddEntity(T entity);
        Task<bool> UpdateEntity(T entity);
        Task<bool> DeleteEntity(int id);
    }
}
