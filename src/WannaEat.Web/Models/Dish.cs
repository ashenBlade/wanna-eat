using System.ComponentModel.DataAnnotations;

namespace WannaEat.Web.Models;

public class Dish: Food
{
    [Required]
    public string Recipe { get; set; }
    public IList<CookingAppliance> CookingAppliances { get; set; }
    public ICollection<DishProduct> ConsistsOf { get; set; }
}