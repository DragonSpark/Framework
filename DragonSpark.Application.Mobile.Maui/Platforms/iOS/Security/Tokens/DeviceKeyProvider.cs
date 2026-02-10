using System.Threading;
using System.Threading.Tasks;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Operations.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class DeviceKeyProvider : IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider() : this(DeviceKey.Default) {}

    readonly IResulting<PublicJWK> _previous;

    public DeviceKeyProvider(DeviceKey key) : this(key.Then().Singleton().Operation().Out()) {}

    public DeviceKeyProvider(IResulting<PublicJWK> previous) => _previous = previous;

    public ValueTask<PublicJWK> Get(CancellationToken ct) => _previous.Get();
}