using WannaEat.Domain.Entities;
using WannaEat.Domain.Interfaces;

namespace WannaEat.Application;

public class AggregatedRecipeProvider: IAggregatedRecipeProvider
{
    private readonly IEnumerable<IRecipeProvider> _recipeServices;

    public AggregatedRecipeProvider(IEnumerable<IRecipeProvider> recipeServices)
    {
        _recipeServices = recipeServices;
    }
    public async Task<IEnumerable<Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients, CancellationToken cancellationToken)
    {
        var recipes =
            await Task.WhenAll(_recipeServices
                                  .Select(s => s.GetRecipesForIngredients(ingredients, cancellationToken)));
        return recipes
           .SelectMany(r => r);
    }
}