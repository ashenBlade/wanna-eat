using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WannaEat.Web.Infrastructure.Attributes;

namespace WannaEat.Web.Dto.Dish;

public class GetDishDto
{
    [FromQuery(Name = "page-number")]
    [Positive]
    [Required]
    public int PageNumber { get; set; }
    
    [FromQuery(Name = "page-size")]
    [Positive]
    [Required]
    public int PageSize { get; set; }

    [FromQuery(Name = "may-contain")]
    public int[] MayContainProductsIds { get; set; } = null!;
}