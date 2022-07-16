namespace WannaEat.Web.Models;

public class Dish: Food
{
    public string Recipe { get; set; }
    public IList<CookingAppliance> CookingAppliances { get; set; }
}