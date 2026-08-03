namespace Models.Dto;

public class MachineAdditionalInformationDto
{
    public int MachineId { get; set; }

    public string? RfidService { get; set; }

    public string? RfidCashCollection { get; set; }

    public int ProductMatrixId { get; set; }

    public string? RfidLoad { get; set; }

    public string TemplateNotifications { get; set; } = null!;

    public string TemplateCriticalValues { get; set; } = null!;

    public string WorkRegime { get; set; } = null!;

    public string WorkTime { get; set; } = null!;

    public string Timezone { get; set; } = null!;

    public string ServicePriority { get; set; } = null!;

    public string? Description { get; set; }

}