using System.ComponentModel.DataAnnotations;

namespace WannaEat.Web.Infrastructure.Attributes;

public class PositiveAttribute: RangeAttribute
{
    public PositiveAttribute(): base(1, int.MaxValue)
    { }
}