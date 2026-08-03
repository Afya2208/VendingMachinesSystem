using Models.Entities;

namespace Models.Domain;

public class StatusDomain
{
    public AdditionalMachineStatus Status { get; set; }
    
    public string Path => $"../../../Assets/Additional/доп{Status.Id - 3}.png";
}