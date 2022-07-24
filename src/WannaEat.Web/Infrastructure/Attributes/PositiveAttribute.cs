using System.ComponentModel.DataAnnotations;

namespace WannaEat.Web.Infrastructure.Attributes;

public class PositiveAttribute: ValidationAttribute
{
    private readonly int _min;

    public PositiveAttribute(int min = 0)
    {
        _min = min;
    }

    public override bool IsValid(object? value)
    {
        return value switch
               {
                   int i       => _min < i,
                   float f     => _min < f,
                   double dou  => _min < dou,
                   byte b      => _min < b,
                   sbyte sb    => _min < sb,
                   uint ui     => _min < ui,
                   decimal dec => _min < dec,
                   long l      => _min < l,
                   short s     => _min < s,
                   ushort us   => _min < us,
                   _           => true
               };
    }
}