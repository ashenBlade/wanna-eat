using Newtonsoft.Json;
using WannaEat.Web.Interfaces;
using WannaEat.Web.Models;

namespace WannaEat.Web.Services;

/// <summary>
/// Recipe Service uses 
/// </summary>
public class MZRRecipeService: IRecipeService
{
    private readonly HttpClient _client;
    private readonly ILogger<MZRRecipeService> _logger;

    public MZRRecipeService(HttpClient client, ILogger<MZRRecipeService> logger)
    {
        _client = client;
        _logger = logger;
    }

    public async Task<IEnumerable<Recipe>> GetRecipesForIngredients(ICollection<Ingredient> ingredients, CancellationToken token)
    {
        var payload = GetPayload(ingredients);
        try
        {
            var response =
                await _client.PostAsync("https://fs2.tvoydnevnik.com/api2/recipe_search/search", payload, token);
            response.EnsureSuccessStatusCode();
            var mzr = await response.Content.ReadFromJsonAsync<MZRResponse>(cancellationToken: token);
            if (mzr is null)
            {
                _logger.LogWarning("Could not parse JSON from response");
                return Enumerable.Empty<Recipe>();
            }
            var recipes = mzr.Result.Recipes
                             .Select(f => new Recipe
                                          {
                                              Link = new Uri(f.Food.Uri),
                                              Name = f.Food.Name,
                                              ImageUrl = new Uri(f.Food.ImageUrl)
                                          });
            return recipes;
        }
        catch (TaskCanceledException canceled)
        {
            _logger.LogWarning(canceled, "Could not download and parse recipes. Task was cancelled from cancellation token");
            return Enumerable.Empty<Recipe>();
        }
        catch (HttpRequestException http)
        {
            _logger.LogWarning(http, "Could not download recipes. Error while HTTP request");
            return Enumerable.Empty<Recipe>();
        }
    }

    private static FormUrlEncodedContent GetPayload(ICollection<Ingredient> ingredients)
    {
        var ingredientsString = string.Join(',', ingredients.Select(i => Uri.EscapeDataString(i.Name)));
        return new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                                         {
                                             new("jwt", "false"), 
                                             new("query[page]", "q"),
                                             new("query[sort][date]", "desc"),
                                             new("query[isEmpty]", "false"), 
                                             new("query[ingredients]", ingredientsString),
                                             new("platformId", "101"),
                                             new("query[count_on_page]", "10")
                                         });
    }

    #region JSON parsing classes
    
    private class MZRResponse
    {
        [JsonProperty("result")]
        public MZRResult Result { get; set; }
    }

    class MZRResult
    {
        [JsonProperty("list")]
        public ICollection<MZRRecipe> Recipes { get; set; }
    }

    class MZRRecipe
    {
        [JsonProperty("food")]
        public MZRFood Food { get; set; }
    }

    class MZRFood
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Uri { get; set; }

        [JsonProperty("photo360200")]
        public string ImageUrl { get; set; }
    }

    #endregion
}