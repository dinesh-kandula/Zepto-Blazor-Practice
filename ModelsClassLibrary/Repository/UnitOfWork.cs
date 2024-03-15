using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ModelsClassLibrary.Models;

namespace ModelsClassLibrary.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ZeptoContext context;

        public IProductRepository ProductRepository { get; private set; }

        public IPizzaRepository PizzaRepository { get; private set; }

        public UnitOfWork(ZeptoContext context)
        {
            this.context = context;
            ProductRepository = new ProductRepository(context);
            PizzaRepository = new PizzaRepository(context);
        }

        public async Task CompleteAsync()
        {
            await this.context.SaveChangesAsync();
        }
    }
}
