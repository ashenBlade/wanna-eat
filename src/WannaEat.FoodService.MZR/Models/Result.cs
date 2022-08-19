using System.Text.Json.Serialization;

namespace WannaEat.FoodService.MZR.Models;

internal class Result
{
    [JsonPropertyName("list")]
    public Recipe[] Recipes { get; set; }
}