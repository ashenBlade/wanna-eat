using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WannaEat.Application;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;
using WannaEat.Infrastructure.Persistence;
using WannaEat.Web.Dto.Recipe;
using WannaEat.Web.Infrastructure.Attributes;


namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/recipes")]
public class RecipesController: ControllerBase
{
    private readonly IAggregatedRecipeProvider _recipeProvider;
    private readonly IIngredientRepository _ingredients;

    public RecipesController(IAggregatedRecipeProvider recipeProvider, IIngredientRepository ingredients)
    {
        _recipeProvider = recipeProvider;
        _ingredients = ingredients;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<GetRecipeDto>>> GetSatisfiedRecipes(
        [FromQuery(Name = "contain")] [Required]
        int[] productIds,
        [FromQuery(Name = "page")] [Required] [Positive]
        int page,
        [FromQuery(Name = "size")] [Required] [Range(1, 100)]
        int size,
        CancellationToken token)
    {
        var ingredients = await _ingredients.FindAllByIdAsync(productIds, token);
        var recipes = await _recipeProvider.GetRecipesForIngredients(ingredients, 40, token);
        return Ok(recipes.Select(r => new GetRecipeDto
                                      {
                                          Name = r.Name,
                                          ImageUrl = r.ImageUrl?.ToString(),
                                          OriginUrl = r.Origin.ToString()
                                      }));
    }
}