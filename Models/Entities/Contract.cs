using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Contract
{
    public int Id { get; set; }

    public string DocumentNumber { get; set; } = null!;

    public DateOnly DateSigned { get; set; }

    public DateOnly DateExpired { get; set; }

    public string Status { get; set; } = null!;

    public int FranchiseeId { get; set; }

    public virtual User Franchisee { get; set; } = null!;
}
