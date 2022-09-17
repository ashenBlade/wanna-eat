using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WannaEat.Domain.Services;
using WannaEat.Web.Dto.Product;
using WannaEat.Web.Infrastructure.Attributes;

namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/ingredients")]
public class IngredientsController: ControllerBase
{
    private readonly IIngredientRepository _ingredients;
    private readonly ILogger<IngredientsController> _logger;

    public IngredientsController(IIngredientRepository ingredients, ILogger<IngredientsController> logger)
    {
        _ingredients = ingredients;
        _logger = logger;
    }

    [HttpGet("")]
    public async Task<ActionResult<GetIngredientDto>> GetIngredientsPaged(
        [FromQuery(Name = "page")]
        [Positive]
        [Required]
        int page,
        [FromQuery(Name = "size")]
        [Range(1, 100)]
        [Required]
        int size,
        CancellationToken token)
    {
        _logger.LogTrace(nameof(GetIngredientsPaged) + "(page={Page}, size={Size}) requested", page, size);
        var ingredientsPaged = await _ingredients.GetIngredientsPaged(page, size, token);
        return Ok(ingredientsPaged);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<GetIngredientDto?>> GetIngredientById(
        [FromRoute(Name = "id")] 
        [Required]
        int id,
        CancellationToken token)
    {
        _logger.LogTrace(nameof(GetIngredientById) + "(id={Id}) requested", id);
        var ingredient = await _ingredients.FindByIdAsync(id, token);
        return ingredient is null
                   ? NotFound()
                   : Ok(new GetIngredientDto()
                        {
                            Id = ingredient.Id,
                            Name = ingredient.Name
                        });
    }

    [HttpGet("search")]
    public async Task<ActionResult<ICollection<GetIngredientDto>>> SearchByName(
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
        int size,
        CancellationToken token)
    {
        _logger.LogTrace(nameof(SearchByName) + "(name={Name},page={Page},size={Size}) requested", name, page, size);
        var ingredients = await _ingredients.FindByNameAsync(name, page, size, token);

        return Ok(ingredients.Select(i => new GetIngredientDto{Id = i.Id, Name = i.Name}));
    }
}