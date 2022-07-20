using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

public class Product: Food
{
    public bool IsFoundational { get; set; }
    public ICollection<DishProduct> RequiredForDish { get; set; }
}