using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("docs")]
public class ContractContoller(VendingDbContext context) : Controller
{
    [HttpGet("contracts/user/{userId:int}")]
    public async Task<IActionResult> Get(int userId)
    {
        return Ok(await context.Contracts.Where(x=>x.FranchiseeId == userId)
            .Select(x=>new
            {
                x.FranchiseeId,
                x.DateExpired,
                x.DateSigned,
                x.Status,
                x.Id,
                x.DocumentNumber
            })
            .ToListAsync());
    }
    [HttpGet("contracts/{contractId:int}")]
    public async Task<IActionResult> GetContract(int contractId)
    {
        return Ok(context.Contracts.FirstOrDefault(x => x.Id == contractId));
    }
    
    [HttpPost("file/{contractId:int}")]
    public async Task<IActionResult> PostFile(int contractId, IFormFile file)
    {
        using (var stream = new MemoryStream())
        {
            await file.CopyToAsync(stream);
            var cont = context.Contracts.Find(contractId);
            context.SaveChanges();
            return Ok();
        }
    }
    [HttpGet("file/{contractId:int}")]
    public async Task<IActionResult> GetFile(int contractId)
    {
        return Ok(context.Contracts.FirstOrDefault(x => x.Id == contractId));
    }
}