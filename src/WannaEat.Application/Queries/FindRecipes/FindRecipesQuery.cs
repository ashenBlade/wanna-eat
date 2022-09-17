using MediatR;
using WannaEat.Domain.Entities;

namespace WannaEat.Application.Queries.FindRecipes;

public class FindRecipesQuery: IRequest<IEnumerable<Recipe>>
{
    public int[] IngredientsIds { get; set; } = null!;
    public int MaxAmount { get; set; }
}