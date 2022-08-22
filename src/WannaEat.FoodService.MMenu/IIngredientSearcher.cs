using WannaEat.Domain.Entities;

namespace WannaEat.FoodService.MMenu;

public interface IIngredientSearcher
{
    Task<IEnumerable<string>> SearchIdsForIngredients(IEnumerable<Ingredient> ingredients, CancellationToken token);
}