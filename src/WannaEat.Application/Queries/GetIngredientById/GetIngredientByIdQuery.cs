using MediatR;
using WannaEat.Domain.Entities;

namespace WannaEat.Application.Queries.GetIngredientById;

public class GetIngredientByIdQuery: IRequest<Ingredient?>
{
    public GetIngredientByIdQuery(int id)
    {
        Id = id;
    }

    public int Id { get; }
}