using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;

namespace WannaEat.Application;

public class AggregatedRecipeProvider: IAggregatedRecipeProvider
{
    private readonly IEnumerable<IRecipeProvider> _recipeServices;

    public AggregatedRecipeProvider(IEnumerable<IRecipeProvider> recipeServices)
    {
        _recipeServices = recipeServices;
    }
    public async Task<IEnumerable<Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients,
                                                                    int max,
                                                                    CancellationToken cancellationToken)
    {
        var recipes =
            await Task.WhenAll(_recipeServices
                                  .Select(s => s.GetRecipesForIngredients(ingredients, max, cancellationToken)));
        return recipes
           .SelectMany(r => r);
    }
}