using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

public class Food
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    public string? ImageUrl { get; set; }
    
    [Required]
    public NutritionalValue NutritionalValue { get; set; }
    [Required]
    public Minerals Minerals { get; set; }
    [Required]
    public Vitamins Vitamins { get; set; }
}