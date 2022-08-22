using System.Text.Json.Serialization;

namespace WannaEat.FoodService.MMenu.Models;

public class SingleIngredientSearchResult
{
    [JsonPropertyName("value")]
    public string Name { get; set; }

    [JsonPropertyName("id")]
    public string Id { get; set; }
}