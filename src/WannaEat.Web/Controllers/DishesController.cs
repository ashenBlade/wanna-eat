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
                                                                           GetDishDto dto)
    {
        var pageNumber = dto.PageNumber;
        var pageSize = dto.PageSize;
        
        _logger.LogTrace("Dishes paged requested");
        var dishes = await _context.Dishes
                                   .Skip(pageSize * ( pageNumber - 1 ))
                                   .Take(pageSize)
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