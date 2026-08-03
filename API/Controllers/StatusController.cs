using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
public class StatusController(VendingDbContext context) : Controller
{
    [HttpGet("work-statuses")]
    public async Task<IActionResult> GetWorkStatuses()
    {
        var workStatusList = await context.MachineWorkStatuses.AsNoTracking().ToListAsync();
        return Ok(workStatusList);
    }
}