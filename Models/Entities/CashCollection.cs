using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class CashCollection
{
    public int Id { get; set; }

    public int MachineId { get; set; }

    public decimal InputSum { get; set; }

    public DateTime DateTime { get; set; }

    public decimal TakenSum { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;
}
