using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

public class Dish: Food
{
    [Required]
    public string Recipe { get; set; }
    public IList<CookingAppliance> RequiredToCook { get; set; }
}