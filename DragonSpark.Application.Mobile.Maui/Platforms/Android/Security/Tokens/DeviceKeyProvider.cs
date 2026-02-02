using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeviceKeyProvider : IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider() : this(DeviceKey.Default.Then().Singleton().Get()) {}

    readonly IResult<PublicJWK> _previous;

    public DeviceKeyProvider(IResult<PublicJWK> previous) => _previous = previous;

    public ValueTask<PublicJWK> Get(CancellationToken ct) => _previous.Get().ToOperation();
}