using System.Text.Json.Serialization;

namespace WannaEat.FoodService.MMenu.Models;

public class ExactIngredientSearchResult
{
    [JsonPropertyName("html")]
    public string? Message { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
}