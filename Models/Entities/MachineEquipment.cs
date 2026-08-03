using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class MachineEquipment
{
    public int MachineId { get; set; }

    public int ChequesStatus { get; set; }

    public int CardsStatus { get; set; }

    public int MonitorStatus { get; set; }

    public int ElectricityStatus { get; set; }

    public int ChangeStatus { get; set; }

    public int ModemStatus { get; set; }

    public int ContactlessPaymentStatus { get; set; }

    public virtual VendingMachine Machine { get; set; } = null!;
}
