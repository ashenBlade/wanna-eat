using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WannaEat.Web.Dto.Product;
using WannaEat.Web.Infrastructure.Attributes;
using WannaEat.Web.Models;
using WannaEat.Web.Services;

namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/ingredients")]
public class IngredientsController: ControllerBase
{
    private readonly WannaEatDbContext _context;
    private readonly ILogger<IngredientsController> _logger;

    public IngredientsController(WannaEatDbContext context, ILogger<IngredientsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<ActionResult<Ingredient>> GetIngredientsPaged(
        [FromQuery(Name = "page")]
        [Positive]
        [Required]
        int page,
        [FromQuery(Name = "size")]
        [Range(1, 100)]
        [Required]
        int size)
    {
        _logger.LogTrace(nameof(GetIngredientsPaged) + "(page={Page}, size={Size}) requested", page, size);
        var ingredientsPaged = await _context.Ingredients
                                             .OrderBy(i => i.Id)
                                             .Skip(( page - 1 ) * size)
                                             .Take(size)
                                             .ToListAsync();
        return Ok(ingredientsPaged);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ReadIngredientDto?>> GetIngredientById(
        [FromRoute(Name = "id")] 
        [Required]
        int id)
    {
        _logger.LogTrace(nameof(GetIngredientById) + "(id={Id}) requested", id);
        var ingredient = await _context.Ingredients
                                       .FindAsync(id);
        return ingredient is null
                   ? NotFound()
                   : Ok(new ReadIngredientDto()
                        {
                            Id = ingredient.Id,
                            Name = ingredient.Name
                        });
    }

    [HttpGet("search")]
    public async Task<ActionResult<ICollection<ReadIngredientDto>>> SearchByName(
        [FromQuery(Name = "name")] 
        [Required] 
        string name,
        [FromQuery(Name = "page")]
        [Positive]
        [Required]
        int page,
        [FromQuery(Name = "size")]
        [Range(1, 100)]
        [Required]
        int size)
    {
        _logger.LogTrace(nameof(SearchByName) + "(name={Name},page={Page},size={Size}) requested", name, page, size);
        var ingredients = await _context.Ingredients
                                        .Where(i => i.NameSearchVector.Matches(EF.Functions.PlainToTsQuery("russian", name)))
                                        .OrderBy(i => i.NameSearchVector.Rank(EF.Functions.PlainToTsQuery("russian", name)))
                                        .ThenBy(i => i.Name)
                                        .Skip(( page - 1 ) * size)
                                        .Take(size)
                                        .ToListAsync();
        
        return Ok(ingredients.Select(i => new ReadIngredientDto{Id = i.Id, Name = i.Name}));
    }
}