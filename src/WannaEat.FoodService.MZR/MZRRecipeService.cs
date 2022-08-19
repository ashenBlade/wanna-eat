using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Interfaces;
using WannaEat.FoodService.MZR.Models;

namespace WannaEat.FoodService.MZR;

public class MZRRecipeService: IRecipeService
{
    private readonly HttpClient _client;
    private readonly ILogger<MZRRecipeService> _logger;

    public MZRRecipeService(HttpClient client, ILogger<MZRRecipeService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<Domain.Entities.Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients, CancellationToken token)
    {
        try
        {
            using var message =
                new HttpRequestMessage(HttpMethod.Post, "https://fs2.tvoydnevnik.com/api2/recipe_search/search");
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            message.Content = GetPayload(ingredients);
            using var response =
                await _client.SendAsync(message, token);
            response.EnsureSuccessStatusCode();
            var mzr = await response
                           .Content
                           .ReadFromJsonAsync<Response>(new JsonSerializerOptions(JsonSerializerDefaults.Web), token);
            if (mzr is null)
            {
                _logger.LogWarning("Could not parse JSON from response");
                return Enumerable.Empty<Domain.Entities.Recipe>();
            }
            var recipes = mzr.Result
                             .Recipes
                             .Select(f => new Domain.Entities.Recipe(f.Food.Name, new Uri($"{f.Domain}{f.Food.Link}"), new Uri(f.Food.ImageUrl)));
            return recipes;
        }
        catch (TaskCanceledException canceled)
        {
            _logger.LogWarning(canceled, "Could not download and parse recipes. Task was cancelled from cancellation token");
            return Enumerable.Empty<Domain.Entities.Recipe>();
        }
        catch (HttpRequestException http)
        {
            _logger.LogWarning(http, "Could not download recipes. Error while HTTP request");
            return Enumerable.Empty<Domain.Entities.Recipe>();
        }
    }

    private static FormUrlEncodedContent GetPayload(IEnumerable<Ingredient> ingredients)
    {
        var ingredientsString = string.Join(',', ingredients.Select(i => i.Name.Trim(' ')));
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