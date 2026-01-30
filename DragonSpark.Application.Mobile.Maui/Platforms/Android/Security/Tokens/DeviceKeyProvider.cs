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

    DeviceKeyProvider() : this(DeviceKey.Default.Then().Singleton().Get()) {}

    readonly IResult<PublicJWK> _previous;

    public DeviceKeyProvider(IResult<PublicJWK> previous) => _previous = previous;

    public ValueTask<PublicJWK> Get(CancellationToken ct) => _previous.Get().ToOperation();
}

sealed class DeviceKey : IResult<PublicJWK>
{
    public static DeviceKey Default { get; } = new();

    DeviceKey() : this(DeterminePoint.Default.Then().Select(DeterminePoints.Default).Get(), ComputeJkt.Default) {}

    readonly IResult<Points>    _points;
    readonly IFormatter<Points> _jkt;

    public DeviceKey(IResult<Points> points, IFormatter<Points> jkt)
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