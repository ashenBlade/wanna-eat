using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WannaEat.Web.Models;

[Owned]
public class Minerals
{
    public double? Potassium { get; set; }
    public double? Calcium { get; set; }
    public double? Magnesium { get; set; }
    public double? Phosphorus { get; set; }
    public double? Sodium { get; set; }
    public double? Zinc { get; set; }
    public double? Iron { get; set; }
    public double? Selenium { get; set; }
    public double? Copper { get; set; }
    public double? Manganese { get; set; }
    public double? Fluorine { get; set; }
    public double? Iodine { get; set; }
    public double? Sulfur { get; set; }
    public double? Chromium { get; set; }
    public double? Silicon { get; set; }
}