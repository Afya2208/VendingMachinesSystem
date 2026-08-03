using Models.Entities;

namespace Models.Dto;

public class CompaniesAndCount
{
    public List<CompanyDto> Companies { get; set; }
    public int TotalCount  { get; set; }
}