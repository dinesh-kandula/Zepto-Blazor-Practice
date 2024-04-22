using TodoModels.Models;

namespace Practice_ADO_Dapper.Services
{
    public interface IUserRepository
    {
        Task<List<Credential>> GetAllAsync();

        Task<List<User>> GetAsync(Guid Id);
    }
}
