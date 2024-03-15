using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using ModelsClassLibrary.Models;

namespace ModelsClassLibrary.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        List<Product> GetAllProductsWithFilter(SqlParameter productParam);
    }
}
