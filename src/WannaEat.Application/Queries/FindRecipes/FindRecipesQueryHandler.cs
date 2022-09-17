using MediatR;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;

namespace WannaEat.Application.Queries.FindRecipes;

public class FindRecipesQueryHandler: IRequestHandler<FindRecipesQuery, IEnumerable<Recipe>>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IAggregatedRecipeProvider _recipeProvider;

    public FindRecipesQueryHandler(IIngredientRepository ingredientRepository, IAggregatedRecipeProvider recipeProvider)
    {
        _ingredientRepository = ingredientRepository;
        _recipeProvider = recipeProvider;
    }
    
    public async Task<IEnumerable<Recipe>> Handle(FindRecipesQuery request, CancellationToken token)
    {
        var ingredients = await _ingredientRepository.FindAllByIdAsync(request.IngredientsIds, token);
        token.ThrowIfCancellationRequested();
        var recipes = await _recipeProvider.GetRecipesForIngredients(ingredients, request.MaxAmount, token);
        token.ThrowIfCancellationRequested();
        return recipes;
    }
}