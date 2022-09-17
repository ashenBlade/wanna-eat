using MediatR;
using WannaEat.Domain.Entities;

namespace WannaEat.Application.Queries.FindIngredientsByName;

public class FindIngredientsByNameQuery: IRequest<IEnumerable<Ingredient>>
{
    public FindIngredientsByNameQuery(string name, int page, int size)
    {
        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page number must be positive");
        }

        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Page size must be positive");
        }
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Page = page;
        Size = size;
    }

    public string Name { get; }
    public int Page { get; }
    public int Size { get; }
}