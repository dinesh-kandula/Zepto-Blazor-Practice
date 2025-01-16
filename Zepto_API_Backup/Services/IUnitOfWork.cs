using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zepto_API_Backup.Services
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }

        IPizzaRepository PizzaRepository { get; }

        IUserRepository UserRepository { get; }

        void BeginTransaction();

        Task RollbackAsync();

        Task CompleteAsync();

        Task CommitAsync();
    }
}
