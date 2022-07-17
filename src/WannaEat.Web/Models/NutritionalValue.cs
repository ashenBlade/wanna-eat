using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WannaEat.Web.Models;

[Owned]
public class NutritionalValue
{
    [Column("kcal")]
    public double? KiloCalories { get; set; }
    [Column("protein")]
    public double? Protein { get; set; }
    [Column("fat")]
    public double? Fat { get; set; }
    [Column("carbs")]
    public double? Carbohydrates { get; set; }
    [Column("water")]
    public double? Water { get; set; }
    [Column("cellulose")]
    public double? Cellulose { get; set; }
    [Column("organic_acids")]
    public double? OrganicAcids { get; set; }
    [Column("glycemic_index")]
    public double? GlycemicIndex { get; set; }
    [Column("cholesterol")]
    public double? Cholesterol { get; set; }
    [Column("saturated_fats")]
    public double? SaturatedFats { get; set; }
}