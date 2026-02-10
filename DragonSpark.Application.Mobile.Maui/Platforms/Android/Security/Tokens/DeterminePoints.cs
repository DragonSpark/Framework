using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using Java.Math;
using Java.Security.Spec;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeterminePoints : ISelect<ECPoint, Points>
{
    public static DeterminePoints Default { get; } = new();

    DeterminePoints() : this(DeterminePoint.Default, FormatPoint.Default) {}

    readonly IResult<ECPoint>       _point;
    readonly IFormatter<BigInteger> _format;

    public DeterminePoints(IResult<ECPoint> point, IFormatter<BigInteger> format)
    {
        _point  = point;
        _format = format;
    }

    public Points Get(ECPoint parameter)
    {
        var point = _point.Get();
        var x     = _format.Get(point.AffineX.Verify());
        var y     = _format.Get(point.AffineY.Verify());
        return new(x, y);
    }
}