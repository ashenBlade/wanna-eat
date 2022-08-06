using WannaEat.Web.Models;

namespace WannaEat.Web.Interfaces;

public interface IRecipeService
{
    public Task<IEnumerable<Recipe>> GetRecipesForIngredients(ICollection<Ingredient> ingredients, CancellationToken cancellationToken);
}