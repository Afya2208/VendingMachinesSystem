using CsvHelper.Configuration;
using Models.Dto;
using Models.Entities;

namespace DesktopApp.Util;

public class VendingMachineCsvMap : ClassMap<VendingMachineShortDto>
{
    public VendingMachineCsvMap()
    {
        Map(m => m.Id);
        Map(m => m.Name);
        Map(m => m.ModelName);
        Map(m => m.OwnerCompanyName);
        Map(m => m.ModemNumber);
        Map(m => m.Address);
        Map(m => m.Place);
        Map(m => m.DateInstalled);
    }
}