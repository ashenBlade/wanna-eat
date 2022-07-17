using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WannaEat.Web.Models;

[Owned]
public class NutritionalValue
{
    public double? KiloCalories { get; set; }
    public double? Protein { get; set; }
    public double? Fat { get; set; }
    public double? Carbohydrates { get; set; }
    public double? Water { get; set; }
    public double? Cellulose { get; set; }
    public double? OrganicAcids { get; set; }
    public double? GlycemicIndex { get; set; }
    public double? Cholesterol { get; set; }
    public double? SaturatedFats { get; set; }
}