using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Application.Mobile.Maui.Storage;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class AccessTokenViewStorage : StorageValue<AccessTokenView?>, IAccessTokenStore
{
    public static AccessTokenViewStorage Default { get; } = new();

    AccessTokenViewStorage() {}
}

// TODO

public sealed class ClearTokenState : Communication.Http.Security.ClearTokenState
{
    public static ClearTokenState Default { get; } = new();

    ClearTokenState() : base(AccessTokenStorage.Default, AccessTokenViewStorage.Default) {}
}