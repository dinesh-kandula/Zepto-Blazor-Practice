namespace Practice_ADO_Dapper.Services
{
    public interface IUnitOfWork
    {
        ITodoRepository TodoRepository { get; }

        IUserRepository UserRepository { get; }
    }
}
