using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WannaEat.Web.Infrastructure.Attributes;
using WannaEat.Web.Interfaces;
using WannaEat.Web.Models;
using WannaEat.Web.Services;

namespace WannaEat.Web.Controllers;

[ApiController]
[Route("api/v1/recipes")]
public class RecipesController: ControllerBase
{
    private readonly ICollection<IRecipeService> _recipeServices;
    private readonly WannaEatDbContext _context;

    public RecipesController(ICollection<IRecipeService> recipeServices, WannaEatDbContext context)
    {
        _recipeServices = recipeServices;
        
        _context = context;
    }

    public async Task<ActionResult<ICollection<Ingredient>>> GetSatisfiedRecipes(
        [FromQuery(Name = "contain")] [Required]
        int[] productIds,
        [FromQuery(Name = "page")] [Required] [Positive]
        int page,
        [FromQuery(Name = "size")] [Required] [Range(1, 100)]
        int size)
    {
        var tokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(5));
        var products = await FindAllProductsByIdsAsync(productIds, page, size);
        var recipes = await Task.WhenAll(_recipeServices.Select(service => service.GetRecipesForIngredients(products, tokenSource.Token)))
                                .ContinueWith(task => task.Result.SelectMany(r => r), tokenSource.Token);
        return Ok(recipes);
    }

    private Task<List<Ingredient>> FindAllProductsByIdsAsync(int[] productIds, int page, int size)
    {
        return _context.Ingredients
                       .Where(i => productIds.Contains(i.Id))
                       .OrderBy(i => i.Id)
                       .Skip(( page - 1 ) * size)
                       .Take(size)
                       .ToListAsync();
    }
}