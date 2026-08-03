using System.Collections;
using Models.Entities;

namespace Models.Dto;

public class AddUpdateVendingMachineDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Number { get; set; }
    public string ModemNumber { get; set; }
    public string Address { get; set; }
    public string Place { get; set; }
    public string? KitOnlineId { get; set; }
    public int OwnerCompanyId { get; set; }
    public DateOnly DateInstalled { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public int ModelId { get; set; }
    public int ManufacturerId { get; set; }
    public int WorkStatusId { get; set; }
    public MachineAdditionalInformationDto MachineAdditionalInformation { get; set; }
    public MachineTechnicalInformationDto MachineTechnicalInformation { get; set; }
    public ICollection<PaymentTypeDto> PaymentTypes { get; set; }
}