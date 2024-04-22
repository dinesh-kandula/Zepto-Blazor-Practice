using TodoModels.Models;

namespace Practice_ADO_Dapper.Services
{
    public interface ITodoRepository
    {
        Task<List<Todo>> GetAllAsync();

        Task<List<Todo>> GetAsync(Guid Id);
    }
}
