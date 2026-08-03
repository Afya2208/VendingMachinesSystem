using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Model
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<VendingMachine> VendingMachines { get; set; } = new List<VendingMachine>();
}
