using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class MachineTechnicalInformation
{
    public int MachineId { get; set; }

    public int CommandsAmount { get; set; }

    public int DetailsAmount { get; set; }

    public decimal ProductLoad { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;
}
