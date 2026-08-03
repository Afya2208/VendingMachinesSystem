using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace API.Controllers;

[ApiController]
[Authorize]
[Route("products")]
public class ProductController(VendingDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await context.Products.ToListAsync();
        return Ok(products);
    }
    [HttpGet("/product-matrices")]
    public async Task<IActionResult> GetProductsMatrices()
    {
        var productsMatrices = await context.ProductMatrices.ToListAsync();
        return Ok(productsMatrices);
    }
}