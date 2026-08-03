using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class VendingMachine
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Number { get; set; } = null!;

    public string ModemNumber { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string Place { get; set; } = null!;

    public string? KitOnlineId { get; set; }

    public int? OwnerCompanyId { get; set; }

    public int? RenterCompanyId { get; set; }

    public DateOnly DateInstalled { get; set; }

    public decimal IncomeCash { get; set; }

    public int ModelId { get; set; }

    public int ManufacturerId { get; set; }

    public decimal ChangeCash { get; set; }

    public int WorkStatusId { get; set; }

    public virtual ICollection<CashCollection> CashCollections { get; set; } = new List<CashCollection>();

    public virtual MachineAdditionalInformation? MachineAdditionalInformation { get; set; }

    public virtual MachineEquipment? MachineEquipment { get; set; }

    public virtual ICollection<MachineProductStorage> MachineProductStorages { get; set; } = new List<MachineProductStorage>();

    public virtual MachineProvider? MachineProvider { get; set; }

    public virtual MachineRentingInformation? MachineRentingInformation { get; set; }

    public virtual MachineTechnicalInformation? MachineTechnicalInformation { get; set; }

    public virtual Manufacturer Manufacturer { get; set; } = null!;

    public virtual Model Model { get; set; } = null!;

    public virtual Company? OwnerCompany { get; set; }

    public virtual Company? RenterCompany { get; set; }

    public virtual ICollection<Reserving> Reservings { get; set; } = new List<Reserving>();

    public virtual ICollection<Sell> Sells { get; set; } = new List<Sell>();

    public virtual ICollection<Service> Services { get; set; } = new List<Service>();

    public virtual MachineWorkStatus WorkStatus { get; set; } = null!;

    public virtual ICollection<ConnectionType> ConnectionTypes { get; set; } = new List<ConnectionType>();

    public virtual ICollection<PaymentType> PaymentTypes { get; set; } = new List<PaymentType>();

    public virtual ICollection<AdditionalMachineStatus> Statuses { get; set; } = new List<AdditionalMachineStatus>();
}
