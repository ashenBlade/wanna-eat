using MediatR;
using WannaEat.Domain.Entities;

namespace WannaEat.Application.Queries.GetIngredientsPaged;

public class GetIngredientsPagedQuery: IRequest<IEnumerable<Ingredient>>
{
    public GetIngredientsPagedQuery(int page, int size)
    {
        if (size < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(size), size, "Page size must be positive");
        }

        if (page < 1)
        {
            throw new ArgumentOutOfRangeException(nameof(page), page, "Page number must be positive");
        }
        
        Size = size;
        Page = page;
    }
    public int Page { get; }
    public int Size { get; }
}