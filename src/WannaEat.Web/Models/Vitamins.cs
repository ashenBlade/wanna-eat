using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WannaEat.Web.Models;

[Owned]
public class Vitamins
{
    [Column("vit_a")]
    public double? A { get; set; }
    [Column("vit_b1")]
    public double? B1 { get; set; }
    [Column("vit_b2")]
    public double? B2 { get; set; }
    [Column("vit_b3_pp")]
    public double? B3PP { get; set; }
    [Column("vit_b4")]
    public double? B4 { get; set; }
    [Column("vit_b5")]
    public double? B5 { get; set; }
    [Column("vit_b6")]
    public double? B6 { get; set; }
    [Column("vit_b9")]
    public double? B9 { get; set; }
    [Column("vit_c")]
    public double? C { get; set; }
    [Column("vit_d")]
    public double? D { get; set; }
    [Column("vit_e")]
    public double? E { get; set; }
    [Column("vit_k")]
    public double? K { get; set; }
    [Column("vit_h")]
    public double? H { get; set; }
}