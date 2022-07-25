using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WannaEat.Web.Dto.Dish;
using WannaEat.Web.Models;
using WannaEat.Web.Services;

namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class DishesController: ControllerBase
{
    private readonly WannaEatDbContext _context;
    private readonly ILogger<DishesController> _logger;

    public DishesController(WannaEatDbContext context, ILogger<DishesController> logger)
    {
        _context = context;
        _logger = logger;
    }
    
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dish>>> GetDishesPagedAsync([Required]
                                                                           [FromQuery]
                                                                           GetDishesDto dto)
    {
        var pageNumber = dto.PageNumber;
        var pageSize = dto.PageSize;
        _logger.LogInformation("Dishes paged requested");
        var offset = pageSize * ( pageNumber - 1 );
        var fetch = pageSize;
        
        var dishes = await _context.Dishes
                                   .Skip(offset)
                                   .Take(fetch)
                                   .ToListAsync();
        
        return Ok(dishes);
    }

    [HttpGet("relevant")]
    public async Task<ActionResult<IEnumerable<Dish>>> GetRelevantDishes([FromQuery]
                                                                         [Required]
                                                                         GetRelevantDishesDto dto)
    {
        var offset = ( dto.PageNumber - 1 ) * dto.PageSize;
        var fetch = dto.PageSize;
        
        var dishes =  await _context.Dishes
                                    .Select(d => new
                                                 {
                                                     Dish = d,
                                                     SatisfiedProductsCount =
                                                         d.ConsistsOf.Count(x => dto.MayContain.Contains(x.ProductId))
                                                 })
                                    .OrderByDescending(x => x.SatisfiedProductsCount)
                                    .Select(x => x.Dish)
                                    .Skip(offset)
                                    .Take(fetch)
                                    .ToListAsync();
        return Ok(dishes);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Dish>> GetDishById(int id)
    {
        _logger.LogTrace("Dish with id {ID} requested", id);
        var dish = await _context.Dishes
                                 .SingleOrDefaultAsync(d => d.Id == id);
        return dish is null
                   ? NotFound()
                   : Ok(dish);
    }
}