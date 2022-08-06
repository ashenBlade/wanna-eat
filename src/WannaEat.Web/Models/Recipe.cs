namespace WannaEat.Web.Models;


public class Recipe
{
    public Uri Link { get; init; }
    public string Name { get; init; }
    public Uri? ImageUrl { get; init; }
    public string Description { get; init; }
}