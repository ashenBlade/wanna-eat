using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

public class CookingAppliance
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string? ImageUrl { get; set; }
    public IList<Dish> Dishes { get; set; }
}