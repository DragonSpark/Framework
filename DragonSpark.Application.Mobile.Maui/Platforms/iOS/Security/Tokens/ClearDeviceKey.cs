using DragonSpark.Application.Model.Values;
using DragonSpark.Application.Security.Tokens;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class ClearDeviceKey : ClearState<PublicJWK>, IClearDeviceKey
{
    public static ClearDeviceKey Default { get; } = new();

    ClearDeviceKey() : base(DeviceKeyProcessStore.Default, DeviceKeyStorageValue.Default) {}
}