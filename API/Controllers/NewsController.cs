using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("news")]
public class NewsController(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetNews()
    {
        var news = await context.News.ToListAsync();
        return Ok(news);
    }
}