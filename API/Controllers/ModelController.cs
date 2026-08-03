using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("models")]
public class ModelController(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetModels()
    {
        var models = await context.Models.ToListAsync();
        return Ok(models);
    }
}