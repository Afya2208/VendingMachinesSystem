using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("notifications")]
public class NotificationController(VendingDbContext context) : Controller
{
    [HttpGet]
    public IActionResult GetNotifications()
    {
        return Ok(context.Notifications.ToList());
    }
   
    [HttpPost]
    public IActionResult PostNotification([FromBody] Notification notification)
    {
        context.Notifications.Add(notification);
        context.SaveChanges();
        return Ok();
    }
}