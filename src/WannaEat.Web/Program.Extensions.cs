using WannaEat.Domain.Interfaces;
using WannaEat.FoodService.MMenu;
using WannaEat.FoodService.MZR;
using WannaEat.Infrastructure.RecipeService;
using WannaEat.Web.Infrastructure.Interfaces;

namespace WannaEat.Web;

public static class ProgramExtensions
{
    public static IServiceCollection AddRecipeServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IIngredientSearcher, ParallelIngredientSearcher>();
        services.AddScoped<IRecipeService, MZRRecipeService>();
        services.AddScoped<IRecipeService, MMenuRecipeService>();
        services.AddScoped<IAggregatedRecipeService, AggregatedRecipeService>();
        return services;
    }
}