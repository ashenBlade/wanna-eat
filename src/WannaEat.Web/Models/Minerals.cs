using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WannaEat.Web.Models;

[Owned]
public class Minerals
{
    [Column("potassium")]
    public double? Potassium { get; set; }
    [Column("calcium")]
    public double? Calcium { get; set; }
    [Column("magnesium")]
    public double? Magnesium { get; set; }
    [Column("phosphorus")]
    public double? Phosphorus { get; set; }
    [Column("sodium")]
    public double? Sodium { get; set; }
    [Column("zinc")]
    public double? Zinc { get; set; }
    [Column("iron")]
    public double? Iron { get; set; }
    [Column("selenium")]
    public double? Selenium { get; set; }
    [Column("copper")]
    public double? Copper { get; set; }
    [Column("manganese")]
    public double? Manganese { get; set; }
    [Column("fluorine")]
    public double? Fluorine { get; set; }
    [Column("iodine")]
    public double? Iodine { get; set; }
    [Column("sulfur")]
    public double? Sulfur { get; set; }
    [Column("chromium")]
    public double? Chromium { get; set; }
    [Column("silicon")]
    public double? Silicon { get; set; }
}