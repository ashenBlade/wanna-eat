using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using WannaEat.Domain.Entities;
using WannaEat.FoodService.MMenu.Models;
using WannaEat.Shared;
using IngredientsSearchResult =
    System.Collections.Generic.Dictionary<int, WannaEat.FoodService.MMenu.Models.SingleIngredientSearchResult>;

namespace WannaEat.FoodService.MMenu;

public class ParallelIngredientSearcher : IIngredientSearcher
{
    private readonly IHttpClientFactory _clientFactory;
    private readonly ILogger<ParallelIngredientSearcher> _logger;

    public ParallelIngredientSearcher(IHttpClientFactory clientFactory, ILogger<ParallelIngredientSearcher> logger)
    {
        _clientFactory = clientFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<string>> SearchIdsForIngredients(IEnumerable<Ingredient> ingredients,
                                                                   CancellationToken token)
    {
        var searchResults = await Task.WhenAll(ingredients.Select(i => SearchIdsForIngredientAsync(i, token)));
        return searchResults
              .SelectMany(result => result)
              .Select(r => r.Id);
    }

    private static HttpRequestMessage CreateHttpRequestMessageForIngredient(Ingredient ingredient)
    {
        return new HttpRequestMessage
               {
                   RequestUri = new UriBuilder(Constants.BaseUrl)
                                {
                                    Path = "/mmtools/ajax/dynamic_list.php",
                                    Query = new UriQueryBuilder
                                            {
                                                Queries = new KeyValuePair<string, string>[]
                                                          {
                                                              new("action", "getIngredientsList"),
                                                              new("ftype", "recipes"), new("fname", ""),
                                                              new("logic", "or"), new("only", "all"),
                                                              new("section", "0"), new("na_kuhnya", "0"),
                                                              new("vid_kuhni", "0"), new("r_cal", "0"),
                                                              new("time_start", "0"), new("time_end", "180"),
                                                              new("city-fname", ""), new("gender", "all"),
                                                              new("term", ingredient.Name.ToLower())
                                                          }
                                            }.Query
                                }.Uri,
                   Headers =
                   {
                       Host = "www.mmenu.com",
                       Referrer = new Uri("https://www.mmenu.com")
                   }
               };
    }

    private async Task<IEnumerable<SingleIngredientSearchResult>> SearchIdsForIngredientAsync(
        Ingredient ingredient,
        CancellationToken token)
    {
        try
        {
            using var client = _clientFactory.CreateClient();
            using var message = CreateHttpRequestMessageForIngredient(ingredient);
            using var response = await client.SendAsync(message, token);
            response.EnsureSuccessStatusCode();
            var x = await response.Content.ReadAsStringAsync(token);
            var searchResult = await response
                                    .Content
                                    .ReadFromJsonAsync<IngredientsSearchResult>(cancellationToken: token);
            if (searchResult is null)
            {
                _logger.LogWarning("Could not parse JSON response for ingredient {IngredientName}. "
                                 + "Returning empty result. "
                                 + "Response: {Response}",
                                   ingredient.Name,
                                   await response.Content.ReadAsStringAsync(token));
                return Enumerable.Empty<SingleIngredientSearchResult>();
            }
            
            return searchResult.Values;
        }
        catch (HttpRequestException request)
        {
            _logger.LogWarning(request,
                               "Could not request ids for ingredient '{IngredientName}'. Error while sending request",
                               ingredient.Name);
            return Enumerable.Empty<SingleIngredientSearchResult>();
        }
        catch (TaskCanceledException cancelled)
        {
            _logger.LogWarning(cancelled, "Could not get ids for ingredient '{IngredientName}'. Task was cancelled",
                               ingredient.Name);
            return Enumerable.Empty<SingleIngredientSearchResult>();
        }
    }
}