using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Models.Entities;

namespace API.Controllers;
[ApiController]
[Authorize]
[Route("machines")]
public class VendingMachineController(VendingDbContext context) : Controller
{
    [HttpGet("{machineId:int}")]
    public async Task<IActionResult> GetMachine(int machineId)
    {
        return Ok(await context.VendingMachines
            .AsNoTracking()
            .Include(x => x.PaymentTypes)
            .Include(x => x.MachineAdditionalInformation)
            .Include(x => x.MachineTechnicalInformation)
            .FirstOrDefaultAsync(x=>x.Id == machineId));
    }
    [HttpGet("pages")]
    public async Task<IActionResult> Pages(int pageNumber, int pageLimit, string? searchText = null)
    {
        var query = context.VendingMachines.AsNoTracking();
        if (searchText != null)
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchText}%")
            || EF.Functions.ILike(x.Number, $"%{searchText}%"));   
        }
        var result = new VendingMachinesAndCount()
        {
            TotalCount = await query.CountAsync()
        };
        result.VendingMachines = await query
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageLimit)
            .Take(pageLimit)
            .Select(x => new VendingMachineShortDto(x.Id, x.Name, x.Model.Name,
                x.OwnerCompany.Name, x.ModemNumber, x.Address, x.Place, x.DateInstalled))
            
            .ToListAsync();
        return Ok(result);
    }
    
    [HttpGet("{userId:int}/pages")]
    public async Task<IActionResult> Pages(int userId, int pageNumber, int pageLimit, string searchText = "")
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var query = context.VendingMachines.AsNoTracking().Where(x=>x.Reservings.Any(y=>y.UserId == userId
        && y.DateEnd <= today));
        if (searchText != null)
        {
            query = query.Where(x => x.Name.Contains(searchText) || x.Number.Contains(searchText) );   
        }
        var result = new VendingMachinesAndCount()
        {
            TotalCount = await query.CountAsync()
        };
        result.VendingMachines = await query.OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageLimit)
            .Take(pageLimit)
            .Select(x => new VendingMachineShortDto(x.Id, x.Name, x.Model.Name,
                x.OwnerCompany.Name, x.ModemNumber, x.Address, x.Place, x.DateInstalled))
            .ToListAsync();
        
        
        
        return Ok(result);
    }
    [HttpGet("export")]
    public async Task<IActionResult> GetAllMachinesForExport()
    {
        var machines = new List<VendingMachineShortDto>();
        machines = await context.VendingMachines
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Select(x => new VendingMachineShortDto(x.Id, x.Name, x.Model.Name,
                x.OwnerCompany.Name, x.ModemNumber, x.Address, x.Place, x.DateInstalled))
            .ToListAsync();
        return Ok(machines);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddUpdateVendingMachineDto machine)
    {
        try
        {
            var newMachine = new VendingMachine()
            {
                MachineAdditionalInformation = new MachineAdditionalInformation() ,
                MachineTechnicalInformation = new MachineTechnicalInformation(),
            };
            context.Entry(newMachine).CurrentValues.SetValues(machine);
            context.Entry(newMachine.MachineAdditionalInformation).CurrentValues.SetValues(machine.MachineAdditionalInformation);
            context.Entry(newMachine.MachineTechnicalInformation).CurrentValues.SetValues(machine.MachineTechnicalInformation);
            foreach (var machinePaymentType in machine.PaymentTypes)
            {
                newMachine.PaymentTypes.Add(await context.PaymentTypes.FindAsync(machinePaymentType.Id));
            }
            context.VendingMachines.Add(newMachine);
            await context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] AddUpdateVendingMachineDto machine)
    {
        try
        {
            var oldMachine = await context.VendingMachines
                .Include(x => x.MachineTechnicalInformation)
                .Include(x => x.MachineAdditionalInformation)
                .Include(x => x.PaymentTypes)
                .FirstOrDefaultAsync(x => x.Id == machine.Id);
            oldMachine.PaymentTypes.Clear();
            context.Entry(oldMachine).CurrentValues.SetValues(machine);
            context.Entry(oldMachine.MachineAdditionalInformation).CurrentValues.SetValues(machine.MachineAdditionalInformation);
            context.Entry(oldMachine.MachineTechnicalInformation).CurrentValues.SetValues(machine.MachineTechnicalInformation);
            foreach (var machinePaymentType in machine.PaymentTypes)
            {
                oldMachine.PaymentTypes.Add(await context.PaymentTypes.FindAsync(machinePaymentType.Id));
            }
            await context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
    [HttpDelete("{machineId:int}")]
    public async Task<IActionResult> Delete(int machineId)
    {
        try
        {
            var deletedRows = await context.VendingMachines
                .Where(m => m.Id == machineId)
                .ExecuteDeleteAsync();
            return Ok(deletedRows == 1);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
    [HttpPatch("{machineId:int}/detach")]
    public async Task<IActionResult> Detach(int machineId)
    {
        try
        {
            var updatedRows = await context.VendingMachines
                .Where(m => m.Id == machineId)
                .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.ModemNumber, "-1"));
            return Ok(updatedRows == 1);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
}