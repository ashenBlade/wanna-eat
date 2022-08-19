using WannaEat.Domain.Entities;

namespace WannaEat.Domain.Interfaces;

public interface IRecipeService
{
    public Task<IEnumerable<Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients, CancellationToken cancellationToken);
}