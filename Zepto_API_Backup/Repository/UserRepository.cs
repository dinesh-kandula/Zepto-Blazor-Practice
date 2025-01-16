using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using ModelsClassLibrary.Models.DTO;
using Zepto_API_Backup.Services;
using Zepto_API_Backup.Context;
using ModelsClassLibrary.Models;

namespace Zepto_API_Backup.Repository
{
    public class UserRepository : GenericRepository<ZeptoUser>, IUserRepository
    {
        private readonly ZeptoPrimaryContext _context;

        public UserRepository(ZeptoPrimaryContext context) : base(context)
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

        public void Update(ZeptoUser user)
        {
            _context.ZeptoUsers.Update(user);
        }

        public List<ZeptoUser> ADOGetAllAsync()
        {
            throw new NotImplementedException();
        }

    }
}
