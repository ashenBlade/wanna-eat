using System.Text.Json.Serialization;

namespace WannaEat.FoodService.MZR.Models;

internal class Recipe
{
    [JsonPropertyName("food")]
    public Food Food { get; set; }
    [JsonPropertyName("domain")]
    public string Domain { get; set; }

    private string Name => Food.Name;
    private Uri ImageUrl => new($"{Domain}{Food.ImageUrl}");

    private Uri Source => new($"{Domain}{Food.Link}");
    
    public Domain.Entities.Recipe ToDomainRecipe() 
        => new(Name, Source, ImageUrl);
}