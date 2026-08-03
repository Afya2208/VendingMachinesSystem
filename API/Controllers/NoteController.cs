using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("notes")]
public class NoteController(VendingDbContext context) : Controller
{
    [HttpPost("notes")]
    public IActionResult Notes([FromBody] Note note)
    {
        context.Notes.Add(note);
        context.SaveChanges();
        return Ok();
    }
}