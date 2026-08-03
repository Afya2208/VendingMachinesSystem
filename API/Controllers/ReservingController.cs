using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("reserving")]
public class ReservingController(VendingDbContext context) : Controller
{
    [HttpGet("dates-for-machine/{machineId}")]
    public async Task<IActionResult> GetDates(int machineId)
    {
        var confirmedReservings = context.Reservings.AsNoTracking().Where(x => x.MachineId == machineId && x.IsConfirmed).ToList();
        var unconfirmedReservings = context.Reservings.AsNoTracking().Where(x => x.MachineId == machineId && !x.IsConfirmed).ToList();
        HashSet<DateOnly> confirmedDates = new HashSet<DateOnly>();
        HashSet<DateOnly> unconfirmedDates = new HashSet<DateOnly>();
        foreach (var reserving in confirmedReservings)
        {
            for (var startDate = reserving.DateStart; startDate <= reserving.DateEnd; startDate = startDate.AddDays(1))
            {
                confirmedDates.Add(startDate);
            }
        }
        foreach (var reserving in unconfirmedReservings)
        {
            for (var startDate = reserving.DateStart; startDate <= reserving.DateEnd; startDate = startDate.AddDays(1))
            {
                unconfirmedDates.Add(startDate);
            }
        }

        var result = new
        {
            takenDays = confirmedDates.ToList(),
            unconfirmedDays = unconfirmedDates.ToList(),
        };
        return Ok(result);
    }
}