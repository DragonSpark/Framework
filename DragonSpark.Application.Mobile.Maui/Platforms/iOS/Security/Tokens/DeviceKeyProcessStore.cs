using DragonSpark.Application.Security.Tokens;
using DragonSpark.Model.Results;

namespace DragonSpark.Application.Mobile.Maui.Platforms.iOS.Security.Tokens;

sealed class DeviceKeyProcessStore : Variable<PublicJWK>
{
    public static DeviceKeyProcessStore Default { get; } = new();

    DeviceKeyProcessStore() {}
}