using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

[Table("dishes")]
public class Dish: Food
{
    [Column("recipe")]
    [Required]
    public string Recipe { get; set; }
    public IList<CookingAppliance> CookingAppliances { get; set; }
}