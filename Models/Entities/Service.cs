using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Service
{
    public int Id { get; set; }

    public int MachineId { get; set; }

    public string Problems { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DateTime { get; set; }

    public int MasterId { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual User Master { get; set; } = null!;
}
