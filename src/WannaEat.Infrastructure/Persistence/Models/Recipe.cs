namespace WannaEat.Infrastructure.Persistence.Models;


public class Recipe
{
    public Uri Link { get; init; }
    public string Name { get; init; }
    public Uri? ImageUrl { get; init; }
}