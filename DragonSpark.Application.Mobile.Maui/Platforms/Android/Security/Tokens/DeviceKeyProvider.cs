using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;
using DragonSpark.Text;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeviceKeyProvider : IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider()
        : this(DeterminePoint.Default.Then().Select(DeterminePoints.Default).Get(), ComputeJkt.Default) {}

    readonly IResult<Points>    _points;
    readonly IFormatter<Points> _jkt;

    public DeviceKeyProvider(IResult<Points> points, IFormatter<Points> jkt)
    {
        _points = points;
        _jkt    = jkt;
    }

    public ValueTask<PublicJWK> Get(CancellationToken ct)
    {
        var points = _points.Get();
        var (x, y) = points;
        var jkt = _jkt.Get(points);
        return new PublicJWK("EC", "P-256", x, y, jkt).ToOperation();
    }
}