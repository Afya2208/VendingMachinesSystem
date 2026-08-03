using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("manufacturers")]
public class ManufacturerController(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetManufacturers()
    {
        var manufacturers = await context.Manufacturers.ToListAsync();
        return Ok(manufacturers);
    }
}