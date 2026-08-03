using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class MachineProductStorage
{
    public int MachineId { get; set; }

    public int ProductId { get; set; }

    public int? Amount { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
