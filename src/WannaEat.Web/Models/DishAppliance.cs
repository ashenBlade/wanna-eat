using System.ComponentModel.DataAnnotations.Schema;

namespace WannaEat.Web.Models;

public class DishAppliance
{
    public int DishId { get; set; }
    public int ApplianceId { get; set; }
}