using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

[Table("cooking_appliances")]
public class CookingAppliance
{
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    [Required]
    public string Name { get; set; }
    [Column("image_url")]
    public string? ImageUrl { get; set; }
}