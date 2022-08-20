namespace WannaEat.FoodService.MMenu.Models;

public class Recipe
{
    public string? Name { get; set; }
    public string? SourceRelativeLink { get; set; }
    public string? ImageUrl { get; set; }
    public RecipeAuthor? Author { get; set; }

    private Uri SourceImageUri => new($"{Constants.BaseUrl}{ImageUrl}");
    private Uri SourceUri => new($"{Constants.BaseUrl}{SourceRelativeLink}");
    public Domain.Entities.Recipe ToDomainRecipe() => new(Name, SourceUri, SourceImageUri);
}