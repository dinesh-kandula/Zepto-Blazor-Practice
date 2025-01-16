using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Zepto_API_Backup.Context;
using Zepto_API_Backup.Services;

namespace Zepto_API_Backup.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ZeptoPrimaryContext context;
        private IDbContextTransaction? _transaction;

        public IProductRepository ProductRepository { get; private set; }

        public IPizzaRepository PizzaRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public UnitOfWork(ZeptoPrimaryContext context)
        {
            this.context = context;
            ProductRepository = new ProductRepository(context);
            PizzaRepository = new PizzaRepository(context);
            UserRepository = new UserRepository(context);
        }

        public void BeginTransaction()
        {
            _transaction = this.context.Database.BeginTransaction();
        }

        public async Task RollbackAsync()
        {
            if(_transaction != null)
            {
                await _transaction.RollbackAsync();
                _transaction = null;
            }
        }

        public async Task CompleteAsync()
        {
            await this.context.SaveChangesAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                _transaction = null;
            }
        }
    }
}
