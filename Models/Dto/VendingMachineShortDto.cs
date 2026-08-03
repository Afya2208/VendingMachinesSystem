namespace Models.Dto;

public record VendingMachineShortDto(
    int Id, string Name, string ModelName, string OwnerCompanyName,
    string ModemNumber, string Address, string Place, DateOnly DateInstalled);