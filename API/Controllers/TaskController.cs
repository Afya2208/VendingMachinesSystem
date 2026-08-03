using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("tasks")]
public class TaskController(VendingDbContext context) : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(context.Tasks.ToList());
    }
}