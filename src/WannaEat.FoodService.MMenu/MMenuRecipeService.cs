﻿using System.Web;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using WannaEat.Domain.Entities;
using WannaEat.Domain.Interfaces;
using WannaEat.FoodService.MMenu.Models;
using Recipe = WannaEat.Domain.Entities.Recipe;

namespace WannaEat.FoodService.MMenu;

public class MMenuRecipeService: IRecipeService
{
    private readonly HttpClient _client;
    private readonly IIngredientSearcher _ingredientSearcher;
    private readonly ILogger<MMenuRecipeService> _logger;

    public MMenuRecipeService(HttpClient client, IIngredientSearcher ingredientSearcher, ILogger<MMenuRecipeService> logger)
    {
        _client = client;
        _ingredientSearcher = ingredientSearcher;
        _logger = logger;
    }
    public async Task<IEnumerable<Recipe>> GetRecipesForIngredients(IEnumerable<Ingredient> ingredients, CancellationToken cancellationToken)
    {
        var html = await DownloadRecipesPageForIngredients(ingredients, cancellationToken);
        _logger.LogInformation("Recipe HTML page was successfully downloaded. Starting parsing");
        var searchResultNode = html.GetElementbyId("search-result");
        if (searchResultNode is null)
        {
            _logger.LogWarning("'#search-result' was not found. Returning empty enumerable");
            return Enumerable.Empty<Recipe>();
        }
        
        var recipeNodes = searchResultNode.ChildNodes
                                          .Where(static node => node.HasClass("mm-element"))
                                          .ToList();
        
        return recipeNodes
              .Select(ParseRecipe)
              .Where(static r => r.Name is not null && 
                                 r.SourceRelativeLink is not null)
              .Select(static x => x.ToDomainRecipe());
    }

    private static Models.Recipe ParseRecipe(HtmlNode node)
    {
        var nameNode = node.ChildNodes.FirstOrDefault(static node => node.HasClass("name"));
        var name = nameNode ?.InnerText;
        var sourceUrl = nameNode?.GetAttributeValue("href", null);
        var authorNode = node.ChildNodes.FirstOrDefault(static node => node.HasClass("element-author"));
        var authorName = authorNode?.InnerText;
        var authorLink = authorNode?.GetAttributeValue("href", null);
        var imgNode = node.ChildNodes
                          .FirstOrDefault(static node => node.HasClass("element-photo"))
                         ?.FirstChild;
        var imageSourceLink = imgNode ?.GetAttributeValue("src", null);
        return new Models.Recipe
               {
                   Author = new RecipeAuthor
                            {
                                Name = authorName, 
                                SourceRelativeUrl = authorLink
                            },
                   ImageUrl = imageSourceLink,
                   Name = name,
                   SourceRelativeLink = sourceUrl
               };
    } 
    
    private async Task<HtmlDocument> DownloadRecipesPageForIngredients(IEnumerable<Ingredient> ingredients, CancellationToken token)
    {
        _logger.LogDebug("Creating HTTP message to request recipes");
        using var message = await GetHttpMessage(ingredients, token);
        _logger.LogDebug("HTTP message to request recipes created. Sending message to server");
        using var response = await _client.SendAsync(message, token);
        var html = await response.Content.ReadAsStringAsync(token);
        
        _logger.LogInformation("Html page with recipes downloaded. Creating HtmlDocument");
        var document = new HtmlDocument();
        document.LoadHtml(html);
        _logger.LogInformation("HtmlDocument successfully created");
        return document;
    }

    private async Task<HttpRequestMessage> GetHttpMessage(IEnumerable<Ingredient> ingredients, CancellationToken token)
    {
        return new HttpRequestMessage(HttpMethod.Get, await GetRequestUri(ingredients, token));
    }

    private async Task<Uri> GetRequestUri(IEnumerable<Ingredient> ingredients, CancellationToken token)
    {
        var ids = await _ingredientSearcher.SearchIdsForIngredients(ingredients, token);
        var queryIds = string.Join('&', ids.Select(static id => $"arrIngr[]={id}") );
        return new UriBuilder(Constants.BaseUrl)
               {
                   Path = "poisk",
                   Query = "ftype=recipes&"
                         + "logic=or&"
                         + "only=all&"
                         + "section=0&"
                         + "nac_kuhnya=0&"
                         + "vid_kuhni=0&"
                         + "r_cal=0&"
                         + "time_start=0&"
                         + "time_end=180&"
                         + "city-fname=&"
                         + "gender=all&"
                         + $"fname=&"
                         + queryIds,
               }.Uri;
    }
}