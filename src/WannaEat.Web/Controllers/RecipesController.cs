using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Interfaces;
using WannaEat.Infrastructure.Persistence;
using WannaEat.Web.Dto.Recipe;
using WannaEat.Web.Infrastructure.Attributes;


namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/recipes")]
public class RecipesController: ControllerBase
{
    private readonly IEnumerable<IRecipeService> _recipeServices;
    private readonly WannaEatDbContext _context;

    public RecipesController(IEnumerable<IRecipeService> recipeServices, WannaEatDbContext context)
    {
        _recipeServices = recipeServices;
        _context = context;
    }

    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<GetRecipeDto>>> GetSatisfiedRecipes(
        [FromQuery(Name = "contain")] [Required]
        int[] productIds,
        [FromQuery(Name = "page")] [Required] [Positive]
        int page,
        [FromQuery(Name = "size")] [Required] [Range(1, 100)]
        int size)
    {
        var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(100));
        var products = await FindAllProductsByIdsAsync(productIds, page, size);
        var recipes = await Task.WhenAll(_recipeServices.Select(service => service.GetRecipesForIngredients(products, tokenSource.Token)))
                                .ContinueWith(task => task.Result.SelectMany(r => r), tokenSource.Token);
        return Ok(recipes.Select(r => new GetRecipeDto
                                      {
                                          Name = r.Name,
                                          ImageUrl = r.ImageUrl?.ToString(),
                                          OriginUrl = r.Origin.ToString()
                                      }));
    }

    private async Task<List<Ingredient>> FindAllProductsByIdsAsync(int[] productIds, int page, int size)
    {
        var satisfiedProducts = await _context.Ingredients
                                              .Where(i => productIds.Contains(i.Id))
                                              .OrderBy(i => i.Id)
                                              .Skip(( page - 1 ) * size)
                                              .Take(size)
                                              .ToListAsync();
        return satisfiedProducts
              .Select(p => new Ingredient(p.Id, p.Name))
              .ToList();
    }
}