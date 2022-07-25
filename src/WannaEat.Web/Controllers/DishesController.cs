using System.ComponentModel.DataAnnotations;
using System.Text;
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

    private static string FormatProductsIds(int[] ids)
    {
        if (ids.Length == 0)
        {
            return "()";
        }

        if (ids.Length == 0)
        {
            return $"({ids[0]})";
        }
        var builder = new StringBuilder("(");
        for (int i = 0; i < ids.Length - 1; i++)
        {
            builder.Append(ids[i]);
            builder.Append(',');
        }

        builder.Append(ids[^1]);
        builder.Append(')');
        return builder.ToString();
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Dish>>> GetDishesPagedAsync([Required]
                                                                           [FromQuery]
                                                                           GetDishDto dto)
    {
        var pageNumber = dto.PageNumber;
        var pageSize = dto.PageSize;
        _logger.LogInformation("Fuck");
        
        _logger.LogTrace("Dishes paged requested");
        IEnumerable<Dish> dishes = null!;
        // dishes = await _context.Dishes
        //                        .Include(d => d.ConsistsOf)
        //                        .Select(d => new {DishId = d.Id, SatisfiedCount = d.ConsistsOf.Where(d => dto
        // .MayContainProductsIds.(d.ProductId))})
        var offset = pageSize * ( pageNumber - 1 );
        var fetch = pageSize;
        if (dto.MayContainProductsIds.Length == 0)
        {
            dishes = await _context.Dishes
                                   .Skip(offset)
                                   .Take(fetch)
                                   .ToListAsync();
        }
        else
        {
            var ids = FormatProductsIds(dto.MayContainProductsIds);
            dishes = await _context.Dishes
                                   .FromSqlRaw(@$"with su as (
    select
        dp.""DishId"" as ""DishId"",
        count(dp.""ProductId"" in {ids}) as ""SatisfiedProductCount""
    from ""DishProducts"" dp
    where dp.""ProductId"" in (3, 4)
    group by dp.""DishId""
)
select f.* from ""Foods"" f
                    join su on su.""DishId"" = f.""Id""
order by su.""SatisfiedProductCount"" desc
offset {offset}
fetch first {fetch} rows only")
                                   .ToListAsync();
        }
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