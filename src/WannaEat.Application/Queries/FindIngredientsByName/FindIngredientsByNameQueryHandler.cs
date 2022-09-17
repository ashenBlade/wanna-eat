using MediatR;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;

namespace WannaEat.Application.Queries.FindIngredientsByName;

public class FindIngredientsByNameQueryHandler: IRequestHandler<FindIngredientsByNameQuery, IEnumerable<Ingredient>>
{
    private readonly IIngredientRepository _ingredientRepository;

    public FindIngredientsByNameQueryHandler(IIngredientRepository ingredientRepository)
    {
        _ingredientRepository = ingredientRepository;
    }
    
    public async Task<IEnumerable<Ingredient>> Handle(FindIngredientsByNameQuery request, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();
        var ingredients = await _ingredientRepository.FindByNameAsync(request.Name, request.Page, request.Size, token);
        token.ThrowIfCancellationRequested();
        return ingredients;
    }
}