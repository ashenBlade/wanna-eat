using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Services;
using WannaEat.FoodService.MZR.Models;
using Recipe = WannaEat.Domain.Entities.Recipe;

namespace WannaEat.FoodService.MZR;

public class MZRRecipeProvider: IRecipeProvider
{
    private readonly HttpClient _client;
    private readonly ILogger<MZRRecipeProvider> _logger;

    public MZRRecipeProvider(HttpClient client, ILogger<MZRRecipeProvider> logger)
    {
        _client = client;
        _logger = logger;
    }

    private static IEnumerable<string> NormalizeIngredientNames(IEnumerable<Ingredient> ingredients)
    {
        return ingredients
           .SelectMany(i => i.Name
                             .ToLower()
                             .Split(' ', StringSplitOptions.RemoveEmptyEntries));
    }

    public async Task<IEnumerable<Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients,
                                                                    int max,
                                                                    CancellationToken token)
    {
        try
        {
            using var message = CreateRequestAllRecipesMessage(ingredients);
            using var response = await _client.SendAsync(message, token);
            response.EnsureSuccessStatusCode();
            var mzr = await response
                           .Content
                           .ReadFromJsonAsync<Response>(new JsonSerializerOptions(JsonSerializerDefaults.Web), token);
            if (mzr is null)
            {
                _logger.LogWarning("Could not parse JSON from response");
                return Enumerable.Empty<Recipe>();
            }
            var recipes = mzr.Result?
                             .Recipes?
                             .Select(f => f.ToDomainRecipe())
                             .Take(max);
            return recipes ?? Enumerable.Empty<Recipe>();
        }
        catch (TaskCanceledException canceled)
        {
            _logger.LogWarning(canceled, "Could not download and parse recipes. "
                                       + "Task was cancelled from cancellation token");
            return Enumerable.Empty<Recipe>();
        }
        catch (HttpRequestException http)
        {
            _logger.LogWarning(http, "Could not download recipes. "
                                   + "Error while HTTP request");
            return Enumerable.Empty<Recipe>();
        }
    }

    private static HttpRequestMessage CreateRequestAllRecipesMessage(IEnumerable<Ingredient> ingredients)
    {
        var message =
            new HttpRequestMessage(HttpMethod.Post, "https://fs2.tvoydnevnik.com/api2/recipe_search/search");
        message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        message.Content = GetPayload(ingredients);
        return message;
    }

    private static FormUrlEncodedContent GetPayload(IEnumerable<Ingredient> ingredients)
    {
        var ingredientsString = string.Join(',', NormalizeIngredientNames(ingredients));
        return new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                                         {
                                             new("jwt", "false"), 
                                             new("query[page]", "1"),
                                             new("query[sort][date]", "desc"),
                                             new("query[ingredients]", ingredientsString),
                                             new("platformId", "101"),
                                             new("query[count_on_page]", "10"),
                                         });
    }
}