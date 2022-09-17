using WannaEat.Domain.Entities;

namespace WannaEat.Domain.Services;

public interface IRecipeProvider
{
    Task<IEnumerable<Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients,
                                                       int max,
                                                       CancellationToken cancellationToken);
}