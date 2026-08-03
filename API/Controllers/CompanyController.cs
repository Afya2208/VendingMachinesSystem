using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Dto;
using Models.Entities;

namespace API.Controllers;
[ApiController]
[Route("companies")]
[Authorize]
public class CompanyController(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetAllCompanies()
    {
        return Ok(await context.Companies.AsNoTracking().ToListAsync());
    }
    [HttpGet("export")]
    public async Task<IActionResult> GetAllCompaniesForExport()
    {
        return Ok(await context.Companies.AsNoTracking().Select(x => new CompanyDto()
        {
            Id = x.Id,
            Address =  x.Address,
            Name = x.Name,
            Contacts = x.Contacts, 
            Description = x.Description,
            DateWorkStarted = x.DateWorkStarted,
            UpperCompanyId = x.UpperCompanyId,
            UpperCompanyName = x.UpperCompany.Name,
        }).ToListAsync());
    }
    
    [HttpGet("pages")]
    public async Task<IActionResult> Pages(int pageNumber, int pageLimit, string? searchText = null)
    {
        var query = context.Companies.AsNoTracking();
        if (searchText != null)
        {
            query = query.Where(x => EF.Functions.ILike(x.Name, $"%{searchText}%")
                                     || EF.Functions.ILike(x.Address, $"%{searchText}%"));   
        }
        var result = new CompaniesAndCount()
        {
            TotalCount = await query.CountAsync()
        };
        result.Companies = await query
            .OrderBy(x => x.Id)
            .Skip((pageNumber - 1) * pageLimit)
            .Take(pageLimit)
            .Select(x => new CompanyDto()
            {
                Id = x.Id,
                Address =  x.Address,
                Name = x.Name,
                Contacts = x.Contacts, 
                Description = x.Description,
                DateWorkStarted = x.DateWorkStarted,
                UpperCompanyId = x.UpperCompanyId,
                UpperCompanyName = x.UpperCompany.Name,
            })
            .ToListAsync();
        return Ok(result);
    }
    
    [HttpGet("{companyId:int}")]
    public async Task<IActionResult> GetCompany(int companyId)
    {
        var company = await context.Companies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == companyId);
        var dto = new CompanyDto()
        {
            Id = company.Id,
            Address =  company.Address,
            Name = company.Name,
            Contacts = company.Contacts, 
            Description = company.Description,
            DateWorkStarted = company.DateWorkStarted,
            UpperCompanyId = company.UpperCompanyId,
        };
        return Ok(dto);
    }
    
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CompanyDto company)
    {
        try
        {
            context.Companies.Add(new Company()).CurrentValues.SetValues(company);
            await context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
    [HttpPut]
    public async Task<IActionResult> Put([FromBody] CompanyDto company)
    {
        try
        {
            var oldCompany = await context.Companies.FindAsync(company.Id);
            context.Entry(oldCompany).CurrentValues.SetValues(company);
            await context.SaveChangesAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }

    [HttpDelete("{companyId:int}")]
    public async Task<IActionResult> Delete(int companyId)
    {
        try
        {
            var rows = await context.Companies.Where(x=> x.Id == companyId).ExecuteDeleteAsync();
            return Ok(rows == 1);
        }
        catch (Exception ex)
        {
            return BadRequest($"{ex.Message}\n{ex.InnerException?.Message}");
        }
    }
}