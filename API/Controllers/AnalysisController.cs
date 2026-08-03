using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
public class AnalysisController(VendingDbContext context) : Controller
{
    [HttpGet("/machines/statuses-info")]
    public async Task<IActionResult> GetMachinesStatusesInfo()
    {
        var query = context.VendingMachines.AsNoTracking().AsQueryable();
        var statuses = new StatusesInfo()
        {
            WorkingAmount =  await query.CountAsync(x=>x.WorkStatusId == 1),
            NotWorkingAmount =  await query.CountAsync(x=>x.WorkStatusId == 2),
            InServiceAmount =  await query.CountAsync(x=>x.WorkStatusId == 3),
        };
        statuses.WorkingPercent = 100.0 * statuses.WorkingAmount / 
                                  (statuses.WorkingAmount + statuses.NotWorkingAmount + statuses.InServiceAmount);
        return Ok(statuses);
    }
    
    [HttpGet("/machines/money-info")]
    public async Task<IActionResult> GetMachinesMoneyInfo()
    {
        var query = context.VendingMachines.AsNoTracking().AsQueryable();
        var moneyInfo = new MoneyInfo()
        {
            TotalCash =  await query.SumAsync(x=>x.IncomeCash),
            TotalChange =  await query.SumAsync(x=>x.ChangeCash),
        };
        return Ok(moneyInfo);
    }
    
    [HttpGet("/machines/services-info")]
    public async Task<IActionResult> GetMachinesServicesInfo()
    {
        var today = DateTime.Now.Date;
        var yesterday = today.AddDays(-1);
        var query = context.Services.AsNoTracking().AsQueryable();
        var serviceInfo = new ServicesInfo()
        {
            ServicesAmountToday = await query.CountAsync(x=>x.DateTime.Date == today),
            ServicesAmountYesterday = await query.CountAsync(x=>x.DateTime.Date == yesterday),
        };
        return Ok(serviceInfo);
    }
    
}