using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WannaEat.Application.Queries.FindIngredientsByName;
using WannaEat.Application.Queries.FindRecipes;
using WannaEat.Application.Queries.GetIngredientById;
using WannaEat.Application.Queries.GetIngredientsPaged;
using WannaEat.Web.Dto.Product;
using WannaEat.Web.Infrastructure.Attributes;

namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/ingredients")]
public class IngredientsController: ControllerBase
{
    private readonly ILogger<IngredientsController> _logger;
    private readonly IMediator _mediator;

    public IngredientsController(ILogger<IngredientsController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
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
        var ingredientsPaged = await _mediator.Send(new GetIngredientsPagedQuery(page, size), token);
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
        var ingredient = await _mediator.Send(new GetIngredientByIdQuery(id), token);
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
        _logger.LogTrace("SearchByName(name={Name},page={Page},size={Size}) requested", name, page, size);
        var query = new FindIngredientsByNameQuery(name, page, size);
        
        var ingredients = await _mediator.Send(query, token);

        return Ok(ingredients.Select(i => new GetIngredientDto{Id = i.Id, Name = i.Name}));
    }
}