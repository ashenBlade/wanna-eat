using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using WannaEat.Web.Infrastructure.Attributes;

namespace WannaEat.Web.Dto.Dish;

public class GetRelevantDishesDto
{
    [Required]
    [Positive]
    [FromQuery(Name = "page-size")]
    public int PageSize { get; set; }
    
    [Required]
    [Positive]
    [FromQuery(Name = "page-number")]
    public int PageNumber { get; set; }
    
    [Required]
    [MinLength(1)]
    [FromQuery(Name = "may-contain")]
    public int[] MayContain { get; set; }
}