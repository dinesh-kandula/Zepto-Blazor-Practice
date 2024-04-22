using Microsoft.EntityFrameworkCore;
using ModelsClassLibrary.Models;
using ModelsClassLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Repository
{
    public class PizzaRepository : GenericRepository<PizzaSpecial>, IPizzaRepository
    {

        public PizzaRepository(ZeptoContext context) : base(context)
        {
            
        }

        public override Task<List<PizzaSpecial>> GetAllAsync()
        {
            return base.GetAllAsync();
        }

        public override Task<PizzaSpecial> GetAsync(int id)
        {
            return DbSet.FirstOrDefaultAsync(item => item.Id == id)!;
        }
    }
}
