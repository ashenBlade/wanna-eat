using System.Text.Json.Serialization;

namespace WannaEat.FoodService.MZR.Models;

internal class Recipe
{
    [JsonPropertyName("food")]
    public Food Food { get; set; }
    [JsonPropertyName("domain")]
    public string Domain { get; set; }
}