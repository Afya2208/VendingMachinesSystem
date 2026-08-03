using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Company
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string Contacts { get; set; } = null!;

    public DateOnly DateWorkStarted { get; set; }

    public int? UpperCompanyId { get; set; }

    public string Address { get; set; } = null!;

    public virtual ICollection<Company> InverseUpperCompany { get; set; } = new List<Company>();

    public virtual Company? UpperCompany { get; set; }

    public virtual ICollection<VendingMachine> VendingMachineOwnerCompanies { get; set; } = new List<VendingMachine>();

    public virtual ICollection<VendingMachine> VendingMachineRenterCompanies { get; set; } = new List<VendingMachine>();
}
