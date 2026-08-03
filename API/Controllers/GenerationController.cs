using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;
[ApiController]
[Authorize]
[Route("gen")]
public class GenerationController(VendingDbContext context) : Controller
{
    private Random _random = new Random();

    private readonly List<string> _providers = ["Mauu", "KobaltP", "GreenStar"];
    
    [HttpGet("provider")]
    public IActionResult Provider()
    {
        var machineProvider = new MachineProvider()
        {
            CompanyName = _providers[_random.Next(0, 3)],
            Sum = _random.Next(100),
            Ping =  _random.Next(1, 6),
        };
        return Ok(machineProvider);
    }
   
    [HttpGet("load")]
    public IActionResult Load()
    {
        return Ok(_random.Next(0, 101));
    }
    [HttpGet("equipment")]
    public IActionResult Equipment()
    {
        var machineEquipment = new MachineEquipment()
        {
            ChangeStatus = _random.Next(-1, 2),
            CardsStatus = _random.Next(-1, 2),
            ContactlessPaymentStatus = _random.Next(-1, 2),
            ModemStatus = _random.Next(-1, 2),
            MonitorStatus = _random.Next(-1, 2),
            ElectricityStatus = _random.Next(-1, 2),
            ChequesStatus = _random.Next(-1, 2),
        };
        return Ok(machineEquipment);
    }
    [HttpGet("statuses")]
    public async Task<IActionResult> Statuses()
    {
        List<AdditionalMachineStatus> statuses = await context.AdditionalMachineStatuses.ToListAsync();
        List<AdditionalMachineStatus> generatedStatuses = new List<AdditionalMachineStatus>();
        HashSet<AdditionalMachineStatus> uniqueStatuses = new ();
        var count = _random.Next(1, statuses.Count + 1);
        if (count == statuses.Count)
        {
            generatedStatuses.AddRange(statuses);
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                var index = _random.Next(statuses.Count);
                uniqueStatuses.Add(statuses[index]);
            }
            generatedStatuses = uniqueStatuses.ToList();
        }
        return Ok(generatedStatuses);
    }
}