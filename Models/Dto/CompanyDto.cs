using Models.Entities;

namespace Models.Dto;

public class CompanyDto
{
    public int Id { get; set; }

    public string Name { get; set; }
    public string Address { get; set; }

    public string? Description { get; set; }

    public string Contacts { get; set; }

    public DateOnly DateWorkStarted { get; set; } = DateOnly.FromDateTime(DateTime.Now);

    public int? UpperCompanyId { get; set; }
    public string? UpperCompanyName { get; set; }
}