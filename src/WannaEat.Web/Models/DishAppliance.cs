using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

[Table("dish_appliance")]
public class DishAppliance
{
    [Column("dish_id")]
    public int DishId { get; set; }
    [Column("appliance_id")]
    public int ApplianceId { get; set; }
}