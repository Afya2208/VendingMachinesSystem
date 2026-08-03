using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("sells")]
public class SellController(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetSells()
    {
        var sells = await context.Sells.ToListAsync();
        return Ok(sells);
    }
    
    [HttpGet("last-10-days")]
    public async Task<IActionResult> GetSellsLast10Days()
    {
        var today = DateTime.Today.Date;
        var nineDaysAgo = today.AddDays(-9);
        var sells = await context.Sells
            .Where(x=> x.DateTime.Date >= nineDaysAgo && x.DateTime.Date <= today)
            .ToListAsync();
        return Ok(sells);
    }
    
    [HttpGet("datetime-range")]
    public async Task<IActionResult> GetSells(DateTime startDateTime, DateTime endDateTime)
    {
        var sells = await context.Sells
            .Where(x=> x.DateTime >= startDateTime && x.DateTime <= endDateTime)
            .ToListAsync();
        return Ok(sells);
    }
}