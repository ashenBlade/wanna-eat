using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

[Table("products")]
public class Product: Food
{
    [Column("is_foundational")]
    public bool IsFoundational { get; set; }
}