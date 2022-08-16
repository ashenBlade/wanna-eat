namespace WannaEat.Web.Dto.Recipe;

public class GetRecipeDto
{
    public string? OriginUrl { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
}