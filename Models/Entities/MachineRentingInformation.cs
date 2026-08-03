using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class MachineRentingInformation
{
    public int MachineId { get; set; }

    public decimal RentPriceMonthly { get; set; }

    public decimal RentPriceYearly { get; set; }

    public int MonthsAmountToGetSuccess { get; set; }

    public string ReservingStatus { get; set; } = null!;

    public virtual VendingMachine Machine { get; set; } = null!;
}
