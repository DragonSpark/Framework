using DragonSpark.Application.Security.Tokens;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeviceKeyProvider : DragonSpark.Model.Operations.Results.Stop.Storing<PublicJWK>, IDeviceKeyProvider
{
    public static DeviceKeyProvider Default { get; } = new();

    DeviceKeyProvider() : base(DeviceKeyProcessStore.Default, DeviceKeyStorage.Default) {}
}