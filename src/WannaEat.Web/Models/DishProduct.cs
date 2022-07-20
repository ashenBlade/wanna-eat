using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

public class DishProduct
{
    public int DishId { get; set; }
    public Dish Dish { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    public string Amount { get; set; }
}