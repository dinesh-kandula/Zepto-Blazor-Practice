using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models
{
    public class Cart
    {
        public int Id { get; set; }

        public Guid ZeptoUserId { get; set; }

        public virtual ZeptoUser? ZeptoUser { get; set; }

        public virtual ICollection<Product>? Products  { get; set; }

    }
}
