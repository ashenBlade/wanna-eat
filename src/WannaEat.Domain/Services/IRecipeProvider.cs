using WannaEat.Domain.Entities;

namespace WannaEat.Domain.Interfaces;

public interface IRecipeProvider
{
    Task<IEnumerable<Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients, CancellationToken cancellationToken);
}