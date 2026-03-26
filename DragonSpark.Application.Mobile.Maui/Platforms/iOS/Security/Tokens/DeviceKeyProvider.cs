using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class DeviceKeyProvider : DragonSpark.Model.Operations.Results.Stop.Storing<PublicJWK>, IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider()
        : base(DeviceKeyProcessStore.Default, CreateDeviceKey.Default.Then().Operation().Out().AsStop()) {}
}