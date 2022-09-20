using System.ComponentModel.DataAnnotations;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using WannaEat.Application.Queries.FindRecipes;
using WannaEat.Web.Dto.Recipe;
using WannaEat.Web.Infrastructure.Attributes;


namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/recipes")]
public class RecipesController: ControllerBase
{
    private readonly IMediator _mediator;

    public RecipesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<GetRecipeDto>>> GetSatisfiedRecipes(
        [FromQuery(Name = "contain")] [Required]
        int[] ingredientIds,
        [FromQuery(Name = "max")] [Positive]
        int max = 20,
        CancellationToken token = default)
    {
        var recipes = await _mediator.Send(new FindRecipesQuery(ingredientIds, max), token);
        return Ok(recipes.Select(r => new GetRecipeDto
                                      {
                                          Name = r.Name,
                                          ImageUrl = r.ImageUrl?.ToString(),
                                          OriginUrl = r.Origin.ToString()
                                      }));
    }
}