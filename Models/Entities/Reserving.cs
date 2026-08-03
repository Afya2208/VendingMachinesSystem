using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Reserving
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int MachineId { get; set; }

    public DateOnly DateStart { get; set; }

    public DateOnly DateEnd { get; set; }

    public bool Insurance { get; set; }

    public string Way { get; set; } = null!;

    public bool IsConfirmed { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
