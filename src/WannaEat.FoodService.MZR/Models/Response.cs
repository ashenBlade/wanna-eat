using System.Text.Json.Serialization;

namespace WannaEat.FoodService.MZR.Models;

internal class Response
{
    [JsonPropertyName("result")]
    public Result Result { get; set; }
}