using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Practice_ADO_Dapper.DBContext
{
    public class SQLDBContext : IDBContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString = string.Empty;
        public string ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
       
        public SQLDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("SqlConnection") ?? string.Empty;
        }

        public IDbConnection CreateConnection() 
            => new SqlConnection(_connectionString);
    }
}
