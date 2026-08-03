using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Sell
{
    public int Id { get; set; }

    public int MachineId { get; set; }

    public decimal Income { get; set; }

    public decimal Change { get; set; }

    public DateTime DateTime { get; set; }

    public int PaymentTypeId { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual PaymentType PaymentType { get; set; } = null!;

    public virtual ICollection<SellProduct> SellProducts { get; set; } = new List<SellProduct>();
}
