using MediatR;
using WannaEat.Domain.Entities;

namespace WannaEat.Application.Queries.GetIngredientById;

public class GetIngredientByIdQuery: IRequest<Ingredient?>
{
    public int Id { get; set; }
}