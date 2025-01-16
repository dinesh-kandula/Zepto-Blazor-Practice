using System.Data;

namespace Practice_ADO_Dapper.DBContext
{
    public interface IDBContext
    {
        string ConnectionString { get; set; }
        IDbConnection CreateConnection();
    }
}
