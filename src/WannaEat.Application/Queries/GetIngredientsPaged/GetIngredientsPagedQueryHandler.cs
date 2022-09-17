using MediatR;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;

namespace WannaEat.Application.Queries.GetIngredientsPaged;

public class GetIngredientsPagedQueryHandler: IRequestHandler<GetIngredientsPagedQuery, IEnumerable<Ingredient>>
{
    private readonly IIngredientRepository _ingredientRepository;

    public GetIngredientsPagedQueryHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    public async Task<IEnumerable<Ingredient>> Handle(GetIngredientsPagedQuery request, CancellationToken token)
    {
        var ingredients =
            await _ingredientRepository.GetIngredientsPaged(request.Page, request.Size, token);
        token.ThrowIfCancellationRequested();
        return ingredients;
    }
}