using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Application.Security.Tokens;
using DragonSpark.Compose;

namespace DragonSpark.Application.Mobile.Maui.Platforms.Android.Security.Tokens;

sealed class DeviceKeyStorageValue : StorageValue<PublicJWK>
{
    public static DeviceKeyStorageValue Default { get; } = new();

    DeviceKeyStorageValue() : base(A.Type<DeviceKeyStorageValue>().FullName.Verify()) {}
}