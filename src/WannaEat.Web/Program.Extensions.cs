using WannaEat.Application;
using WannaEat.Domain.Services;
using WannaEat.FoodService.MMenu;
using WannaEat.FoodService.MZR;

namespace WannaEat.Web;

public static class ProgramExtensions
{
    public static IServiceCollection AddRecipeProviders(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<IIngredientSearcher, ParallelIngredientSearcher>();
        services.AddScoped<IRecipeProvider, MZRRecipeProvider>();
        services.AddScoped<IRecipeProvider, MMenuRecipeProvider>();
        services.AddScoped<IAggregatedRecipeProvider, AggregatedRecipeProvider>();
        return services;
    }
}