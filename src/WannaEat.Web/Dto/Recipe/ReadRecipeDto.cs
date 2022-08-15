namespace WannaEat.Web.Dto.Dish;

public class ReadRecipeDto
{
    public string? OriginUrl { get; set; }
    public string Name { get; set; } = null!;
    public string? ImageUrl { get; set; }
}