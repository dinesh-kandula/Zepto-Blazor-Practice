using Microsoft.Data.SqlClient;
using ModelsClassLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsClassLibrary.Models.DTO;

namespace Zepto_API_Backup.Services
{
    public interface IUserRepository
    {
        List<ZeptoUser> ADOGetAllAsync();

        Task<ZeptoUser> ADOGetUserAsync(string userName);

        void Update(ZeptoUser user);

        Task<Guid> ADOCreateAsync(ZeptoUserDTO user);
    }
}
