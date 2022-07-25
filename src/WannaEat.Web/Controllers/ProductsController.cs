using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WannaEat.Web.Infrastructure.Attributes;
using WannaEat.Web.Models;
using WannaEat.Web.Services;

namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly WannaEatDbContext _context;
    private readonly ILogger<ProductsController> _logger;

    public ProductsController(WannaEatDbContext context, ILogger<ProductsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<Product>>> GetFoodPaged([Required(ErrorMessage = "Specify page size")]
                                                                    [FromQuery(Name = "s")]
                                                                    [Range(1, 100)]
                                                                    int pageSize, 
                                                                    [Required(ErrorMessage = "Specify page number")]
                                                                    [FromQuery(Name = "n")]
                                                                    [Range(1, int.MaxValue)]
                                                                    int pageNumber)
    {
        _logger.LogTrace("Products paged requested");
        var products = await _context.Products
                                     .Skip(pageSize * ( pageNumber - 1 ))
                                     .Take(pageSize)
                                     .ToListAsync();
        return Ok(products);
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Product>>> GetProductsByName([FromQuery(Name = "name")][Required][MinLength(3)]
                                                                            string name, 
                                                                            [FromQuery(Name = "max")][Range(1, 100)]
                                                                            int max = 10)
    {
        _logger.LogInformation("Search product by name '{ProductName}' requested with max amount {MaxAmount}", name, max);
        var products = await _context.Products
                                     .Where(p => EF.Functions.ILike(p.Name, name))
                                     .Take(max)
                                     .ToListAsync();
        _logger.LogDebug("{ProductsAmount} found by query {ProductName}", products.Count, name);
        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Product>> GetProductByIdAsync([FromRoute(Name = "id")][Required]
                                                                 int id)
    {
        _logger.LogTrace("Product with id {ID} requested", id);
        var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == id);
        if (product is null)
        {
            return NotFound();
        }

        return Ok(product);
    }
}

