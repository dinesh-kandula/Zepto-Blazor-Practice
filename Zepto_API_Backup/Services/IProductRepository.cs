using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using ModelsClassLibrary.Models;

namespace Zepto_API_Backup.Services
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        List<Product> GetAllProductsWithFilter(SqlParameter productParam);

        List<Product> ADOGetAllAsync(String? productName);
    }
}
