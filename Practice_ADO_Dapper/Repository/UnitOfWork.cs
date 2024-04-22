using Practice_ADO_Dapper.DBContext;
using Practice_ADO_Dapper.Services;

namespace Practice_ADO_Dapper.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SQLDBContext _dBContext;

        public ITodoRepository TodoRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public UnitOfWork(SQLDBContext dBContext)
        {
            _dBContext = dBContext;
            TodoRepository = new TodoRepository(dBContext);
            UserRepository = new UserRepository(dBContext); 
        }
    }
}
