using System.Net.Http.Json;
using HtmlAgilityPack;
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
           .Where(id => id is not null)!;
    }


    private async Task<string?> SearchIdsForIngredientAsync(
        Ingredient ingredient,
        CancellationToken token)
    {
        try
        {
            using var client = _clientFactory.CreateClient();
            var exactNameSearchResult = await SearchSingleIdForIngredientAsync(ingredient, client, token);
            if (exactNameSearchResult is not null)
            {
                return exactNameSearchResult;
            }

            return await SearchIdListForIngredientAsync(ingredient, client, token);
        }
        catch (HttpRequestException request)
        {
            _logger.LogWarning(request,
                               "Could not request ids for ingredient '{IngredientName}'. "
                             + "Error while sending request",
                               ingredient.Name);
            return null;
        }
        catch (TaskCanceledException cancelled)
        {
            _logger.LogWarning(cancelled, "Could not get ids for ingredient '{IngredientName}'. "
                                        + "Task was cancelled",
                               ingredient.Name);
            return null;
        }
    }


    private async Task<string?> SearchIdListForIngredientAsync(Ingredient ingredient,
                                                               HttpClient client,
                                                               CancellationToken token)
    {
        using var message = CreateHttpRequestMessageForIngredientList(ingredient);
        using var response = await client.SendAsync(message, token);
        response.EnsureSuccessStatusCode();
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
            return null;
        }

        try
        {
            var searchIdListForIngredientAsync = searchResult.Values.Aggregate((m, c) => m.Name.Length > c.Name.Length
                                                                                             ? c
                                                                                             : m)
                                                             .Name;
            return searchIdListForIngredientAsync;
        }
        catch (InvalidOperationException)
        {
            _logger.LogDebug("Empty result ID list returned from query for ingredient {Ingredient}", ingredient.Name);
            return null;
        }
    }

        
    
    private async Task<string?> SearchSingleIdForIngredientAsync(Ingredient ingredient, 
                                                                 HttpClient client,
                                                                 CancellationToken token)
    {
        try
        {
            using var message = CreateHttpRequestMessageForConcreteIngredient(ingredient);
            using var response = await client.SendAsync(message, token);
            response.EnsureSuccessStatusCode();
            var result = await response.Content
                                       .ReadFromJsonAsync<ExactIngredientSearchResult>(cancellationToken: token);
            if (result is null)
            {
                _logger.LogWarning("Could not parse concrete ingredient search result");
                return null;
            }

            if (result.Status is not "ok" || result.Message is null)
            {
                _logger.LogWarning("Could not find best ingredient. "
                                 + "Return status is \"{ReturnStatus}\". "
                                 + "Message: \"{Message}\"",
                                   result.Status, result.Message);
                return null;
            }

            var id = ParseSingleIngredientId(result.Message);
            if (id is null)
            {
                _logger.LogWarning("Id for single ingredient could not be found. "
                                 + "Message: {Message}", result.Message);
                return null;
            }

            _logger.LogInformation("For single ingredient {IngredientName} found id {FoundId}", ingredient.Name, id);

            return id;
        }
        catch (TaskCanceledException canceled)
        {
            _logger.LogWarning(canceled, "Could not get id for single ingredient. Task was cancelled");
            return null;
        }
        catch (HttpRequestException httpRequest)
        {
            _logger.LogWarning(httpRequest, "Could not get id for single ingredient. Error in HTTP Request");
            return null;
        }
        catch (NodeAttributeNotFoundException nodeNotFound)
        {
            _logger.LogWarning(nodeNotFound, "Could not found value attribute in <input> tag for concrete ingredient");
            return null;
        }
        catch (NodeNotFoundException nodeNotFound)
        {
            _logger.LogWarning(nodeNotFound, "Could not find <input> node for concrete ingredient");
            return null;
        }

        string? ParseSingleIngredientId(string html)
        {
            var node = HtmlNode.CreateNode(html);
            var input = node.SelectSingleNode(".//input");
            return input.GetAttributeValue("value", null);
        }
    }

    private static HttpRequestMessage CreateHttpRequestMessageForConcreteIngredient(Ingredient ingredient)
    {
        return new HttpRequestMessage()
               {
                   RequestUri = new UriBuilder(Constants.BaseUrl)
                                {
                                    Path = "/mmtools/ajax/dynamic_list.php",
                                    Query = new UriQueryBuilder
                                            {
                                                Queries = new KeyValuePair<string, string>[]
                                                          {
                                                              new("action", "getIngredient"),
                                                              new("fname", ingredient.Name.ToLower()), 
                                                              new("mode", "add"),
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
    
    private static HttpRequestMessage CreateHttpRequestMessageForIngredientList(Ingredient ingredient)
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
                                                              new("term", TrimLastCharacter(ingredient))
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

    private static string TrimLastCharacter(Ingredient ingredient)
    {
        return ingredient.Name.ToLower();
    }
}