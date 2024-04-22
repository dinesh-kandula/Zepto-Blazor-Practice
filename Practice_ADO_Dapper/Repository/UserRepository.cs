using Dapper;
using Practice_ADO_Dapper.DBContext;
using Practice_ADO_Dapper.Services;
using TodoModels.Models;

namespace Practice_ADO_Dapper.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly SQLDBContext _context;

        public UserRepository(SQLDBContext context)
        {
            _context = context;
        }

        public async Task<List<Credential>> GetAllAsync()
        {
            using var connection = _context.CreateConnection();
            {
                var query = """
                        SELECT * FROM Credentials C
                            INNER JOIN Users u on c.UserId = u.UserId
                    """;
                var users = await connection.QueryAsync<Credential, User, Credential>(query,
                        (credential, user) =>
                        {
                            credential.User = user;
                            return credential;
                        },
                        splitOn: " UserId"
                    );

                users.ToList().ForEach(user => Console.WriteLine($"username: {user.Username}, password: {user.Password}, user:  {user.User}, userId:  {user.Id}"));

                return users.ToList();
            }
        }

        public Task<List<User>> GetAsync(Guid Id)
        {
            throw new NotImplementedException();
        }
    }
}
