using System;
using System.Collections.Generic;

namespace ModelsClassLibrary.Models;

public partial class Cart
{
    public int Id { get; set; }

    public Guid ZeptoUserId { get; set; }

    public virtual ZeptoUser ZeptoUser { get; set; } = null!;

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
