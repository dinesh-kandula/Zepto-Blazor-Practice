using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zepto_API_Backup.Context;
using ModelsClassLibrary.Models;
using Zepto_API_Backup.Services;

namespace Zepto_API_Backup.Repository
{
    public class PizzaRepository : GenericRepository<PizzaSpecial>, IPizzaRepository
    {

        public PizzaRepository(ZeptoPrimaryContext context) : base(context)
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
