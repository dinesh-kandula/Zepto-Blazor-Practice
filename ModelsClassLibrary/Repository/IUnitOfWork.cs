using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Repository
{
    public interface IUnitOfWork
    {
        IProductRepository ProductRepository { get; }

        IPizzaRepository PizzaRepository { get; }

        Task CompleteAsync();
    }
}
