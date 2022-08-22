namespace WannaEat.FoodService.MMenu.Models;

public class Recipe
{
    public string? Name { get; set; }
    public string? SourceAbsolutePath { get; set; }
    public string? ImageAbsolutePath { get; set; }
    public RecipeAuthor? Author { get; set; }

    private Uri SourceImageUri => new($"{Constants.BaseUrl}{ImageAbsolutePath}");
    private Uri SourceUri => new($"{Constants.BaseUrl}{SourceAbsolutePath}");
    public Domain.Entities.Recipe ToDomainRecipe() => new(Name, SourceUri, SourceImageUri);
}