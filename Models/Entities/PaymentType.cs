using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class PaymentType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Sell> Sells { get; set; } = new List<Sell>();

    public virtual ICollection<VendingMachine> Machines { get; set; } = new List<VendingMachine>();
}
