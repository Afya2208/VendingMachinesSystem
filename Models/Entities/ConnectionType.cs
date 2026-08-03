using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class ConnectionType
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<VendingMachine> Machines { get; set; } = new List<VendingMachine>();
}
