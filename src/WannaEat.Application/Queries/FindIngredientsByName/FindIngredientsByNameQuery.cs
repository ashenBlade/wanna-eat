using MediatR;
using WannaEat.Domain.Entities;

namespace WannaEat.Application.Queries.FindIngredientsByName;

public class FindIngredientsByNameQuery: IRequest<IEnumerable<Ingredient>>
{
    public string Name { get; set; } = null!;
    public int Page { get; set; } = 1;
    public int Size { get; set; } = 15;
}