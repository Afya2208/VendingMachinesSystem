using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class MachineProvider
{
    public int MachineId { get; set; }

    public string CompanyName { get; set; } = null!;

    public int Ping { get; set; }

    public decimal Sum { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;
}
