using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class CreateDeviceKey : IResult<PublicJWK>
{
    public static CreateDeviceKey Default { get; } = new();

    CreateDeviceKey() : this(DeterminePoint.Default.Then().Select(DeterminePoints.Default).Get(), ComputeJkt.Default) {}

    readonly IResult<Points>    _points;
    readonly IFormatter<Points> _jkt;

    public CreateDeviceKey(IResult<Points> points, IFormatter<Points> jkt)
    {
        _points = points;
        _jkt    = jkt;
    }

    public PublicJWK Get()
    {
        var points = _points.Get();
        var (x, y) = points;
        var jkt = _jkt.Get(points);
        return new("EC", "P-256", x, y, jkt);
    }
}