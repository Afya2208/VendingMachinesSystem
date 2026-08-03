using CsvHelper.Configuration;
using Models.Dto;

namespace DesktopApp.Util;

public class CompanyCsvMap : ClassMap<CompanyDto>
{
    public CompanyCsvMap()
    {
        Map(m => m.Id);
        Map(m => m.Name);
        Map(m => m.Address);
        Map(m => m.Contacts);
        Map(m => m.DateWorkStarted);
        Map(m => m.Description);
        Map(m => m.UpperCompanyId);
        Map(m => m.UpperCompanyName);
    }
}