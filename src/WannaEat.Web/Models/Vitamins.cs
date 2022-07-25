using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace WannaEat.Web.Models;

[Owned]
[JsonObject(ItemNullValueHandling = NullValueHandling.Ignore)]
public class Vitamins
{
    public double? A { get; set; }
    public double? B1 { get; set; }
    public double? B2 { get; set; }
    public double? B3PP { get; set; }
    public double? B4 { get; set; }
    public double? B5 { get; set; }
    public double? B6 { get; set; }
    public double? B9 { get; set; }
    public double? C { get; set; }
    public double? D { get; set; }
    public double? E { get; set; }
    public double? K { get; set; }
    public double? H { get; set; }
}