using Dapper;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ModelsClassLibrary.Models.DTO;

namespace ModelsClassLibrary.Repository
{
    public class UserRepository : GenericRepository<ZeptoUser>, IUserRepository
    {
        private readonly ZeptoContext _context;

        public UserRepository(ZeptoContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Guid> ADOCreateAsync(ZeptoUserDTO user)
        {
            var guid = Guid.NewGuid();
            var hasedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
           
            var inputs = new DynamicParameters();
            inputs.Add("@id", guid);
            inputs.Add("@userName", user.UserName);
            inputs.Add("@password", hasedPassword);
            inputs.Add("@email", user.Email);
            inputs.Add("@fullName", user.FullName);
            inputs.Add("@gender", user.Gender.ToString());
            inputs.Add("@address", user.Address);
            inputs.Add("@phone", user.Phone);
            inputs.Add("@userType", user.UserType.ToString());

            using (var connection = _context.CreateConnection())
            {
                //createZeptoUser - stored procedure, Create a new user and returns the newly added user Id, if anything fails it returns NULL
                var result = await connection.QueryAsync("createZeptoUser", inputs, commandType: CommandType.StoredProcedure);
                return (Guid) result.ToList()[0].Id;
            }
        }

        public async Task<ZeptoUser> ADOGetUserAsync(string userName)
        {
            var query = $"SELECT * FROM ZeptoUser WHERE UserName='{userName}';";

            var inputs = new DynamicParameters();
            inputs.Add("@UserName", userName);

            using (var connection = _context.CreateConnection())
            {
                var result = await connection.QueryFirstOrDefaultAsync<ZeptoUser>(query, inputs, commandType: CommandType.Text);
                return result;
            }
        }
        
        public List<ZeptoUser> ADOGetAllAsync()
        {
            throw new NotImplementedException();
        }

    }
}
