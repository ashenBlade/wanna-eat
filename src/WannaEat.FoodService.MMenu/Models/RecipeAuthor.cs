namespace WannaEat.FoodService.MMenu.Models;

public class RecipeAuthor
{
    public string? Name { get; set; }
    public string? SourceAbsolutePath { get; set; }
    public Uri SourceLink => new($"{Constants.BaseUrl}{SourceAbsolutePath}");
}