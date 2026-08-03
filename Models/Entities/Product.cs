using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal Popularity { get; set; }

    public int MinimalAmountInStorage { get; set; }

    public virtual ICollection<MachineProductStorage> MachineProductStorages { get; set; } = new List<MachineProductStorage>();

    public virtual ICollection<SellProduct> SellProducts { get; set; } = new List<SellProduct>();
}
