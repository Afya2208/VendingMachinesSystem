using Models.Entities;

namespace Models.Dto;

public class VendingMachinesAndCount
{
    public int TotalCount { get; set; }
    public List<VendingMachineShortDto> VendingMachines { get; set; }
}