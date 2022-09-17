using MediatR;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;

namespace WannaEat.Application.Queries.GetIngredientById;

public class GetIngredientByIdQueryHandler: IRequestHandler<GetIngredientByIdQuery, Ingredient?>
{
    private readonly IIngredientRepository _ingredientRepository;

    public GetIngredientByIdQueryHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    public async Task<Ingredient?> Handle(GetIngredientByIdQuery request, CancellationToken token)
    {
        var ingredient = await _ingredientRepository.FindByIdAsync(request.Id, token);
        token.ThrowIfCancellationRequested();
        return ingredient;
    }
}