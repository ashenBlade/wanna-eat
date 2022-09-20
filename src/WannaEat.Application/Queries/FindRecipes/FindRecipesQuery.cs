using MediatR;
using WannaEat.Domain.Entities;

namespace WannaEat.Application.Queries.FindRecipes;

public class FindRecipesQuery: IRequest<IEnumerable<Recipe>>
{
    public FindRecipesQuery(int[] ingredientsIds, int maxAmount)
    {
        if (maxAmount < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(maxAmount), maxAmount,
                                                  "Max amount of recipes must be positive");
        }
        IngredientsIds = ingredientsIds ?? throw new ArgumentNullException(nameof(ingredientsIds));
        MaxAmount = maxAmount;
    }

    public int[] IngredientsIds { get; }
    public int MaxAmount { get; }
}