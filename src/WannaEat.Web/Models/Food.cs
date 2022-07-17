using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

[Table("foods")]
public class Food
{
    [Column("id")]
    [Key]
    public int Id { get; set; }
    
    [Column("name")]
    [Required]
    public string Name { get; set; }
    
    [Column("image_url")]
    public string ImageUrl { get; set; }
    
    [Required]
    public NutritionalValue NutritionalValue { get; set; }
    [Required]
    public Minerals Minerals { get; set; }
    [Required]
    public Vitamins Vitamins { get; set; }
}