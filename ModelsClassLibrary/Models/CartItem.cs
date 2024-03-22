﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsClassLibrary.Models
{
    public class CartItem
    {
        public int Id { get; set; } 
        public int CartId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public virtual Cart? Cart { get; set; }
        public virtual Product? Product { get; set; }

    }
}
