using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

/*sealed class DeviceKeyProvider : IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider() : this(CreateDeviceKey.Default) {}

    readonly IResulting<PublicJWK> _previous;

    public DeviceKeyProvider(IResult<PublicJWK> key) : this(key.Then().Singleton().Operation().Out()) {}

    public DeviceKeyProvider(IResulting<PublicJWK> previous) => _previous = previous;

    public ValueTask<PublicJWK> Get(CancellationToken ct) => _previous.Get();
}*/
sealed class DeviceKeyProvider : DragonSpark.Model.Operations.Results.Stop.Storing<PublicJWK>, IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider()
        : base(DeviceKeyProcessStore.Default, CreateDeviceKey.Default.Then().Operation().Out().AsStop()) {}
}
// TODO
sealed class DeviceKeyProcessStore : Variable<PublicJWK>
{
    public static DeviceKeyProcessStore Default { get; } = new();

    DeviceKeyProcessStore() {}
}