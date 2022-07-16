using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

public class Food
{
    public int Id { get; set; }
    public string Name { get; set; }
    [Column("image_url")]
    public string ImageUrl { get; set; }
    public NutritionalValue NutritionalValue { get; set; }
    public Minerals Minerals { get; set; }
    public Vitamins Vitamins { get; set; }
}