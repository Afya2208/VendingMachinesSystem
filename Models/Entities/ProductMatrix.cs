using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class ProductMatrix
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<MachineAdditionalInformation> MachineAdditionalInformations { get; set; } = new List<MachineAdditionalInformation>();
}
