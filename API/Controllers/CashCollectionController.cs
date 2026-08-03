using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("cash-collections")]
public class CashCollectionController(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetCashCollections()
    {
        var cashCollections = await context.CashCollections.ToListAsync();
        return Ok(cashCollections);
    }
}