using System;
using System.Collections.Generic;

namespace ModelsClassLibrary.Models;

public partial class PizzaSpecial
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public decimal BasePrice { get; set; }

    public string? Description { get; set; }

    public string? ImageUrl { get; set; }
}
