using System.Text.Json.Serialization;

namespace WannaEat.FoodService.MZR.Models;

internal class Food
{
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("url")]
    public string Link { get; set; }

    [JsonPropertyName("photo360200")]
    public string ImageUrl { get; set; }
}