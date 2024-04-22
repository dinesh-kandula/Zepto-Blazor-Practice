using Microsoft.Data.SqlClient;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Services
{
    public interface IUserRepository
    {
        List<ZeptoUser> ADOGetAllAsync();

        Task<ZeptoUser> ADOGetUserAsync(string userName);

        Task<Guid> ADOCreateAsync(ZeptoUserDTO user);
    }
}
