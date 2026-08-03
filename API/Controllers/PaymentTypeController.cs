using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("payment-types")]
public class PaymentType(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetPaymentTypes()
    {
        var paymentTypes = await context.PaymentTypes.AsNoTracking()
            .Select(x=> new PaymentTypeDto()
            {
                Id = x.Id,
                Name = x.Name,
            }).ToListAsync();
        return Ok(paymentTypes);
    }
}