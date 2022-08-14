using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.WebUtilities;
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
        try
        {
            using var message =
                new HttpRequestMessage(HttpMethod.Post, "https://fs2.tvoydnevnik.com/api2/recipe_search/search");
            message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // message.Headers.Host = string.Empty;
            message.Content = GetPayload(ingredients);
            using var response =
                await _client.SendAsync(message, token);
            response.EnsureSuccessStatusCode();
            // var str = await response.Content.ReadAsStringAsync(token);
            var mzr = await response.Content.ReadFromJsonAsync<MZRResponse>(new JsonSerializerOptions(JsonSerializerDefaults.Web), token);
            if (mzr is null)
            {
                _logger.LogWarning("Could not parse JSON from response");
                return Enumerable.Empty<Recipe>();
            }
            var recipes = mzr.Result
                             .Recipes
                             .Select(f => new Recipe
                                          {
                                              Link = new Uri($"{f.Domain}{f.Food.Link}"),
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

    #region JSON parsing classes
    
    private class MZRResponse
    {
        [JsonPropertyName("result")]
        public MZRResult Result { get; set; }
    }

    private class MZRResult
    {
        [JsonPropertyName("list")]
        public MZRRecipe[] Recipes { get; set; }
    }

    private class MZRRecipe
    {
        [JsonPropertyName("food")]
        public MZRFood Food { get; set; }
        [JsonPropertyName("domain")]
        public string Domain { get; set; }
    }

    private class MZRFood
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Link { get; set; }

        [JsonPropertyName("photo360200")]
        public string ImageUrl { get; set; }
    }

    #endregion
}