using DragonSpark.Application.Communication.Http.Security;
using DragonSpark.Application.Mobile.Maui.Storage;
using DragonSpark.Contracts.Security;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class AccessTokenViewStorage : StorageValue<AccessTokenView>, IAccessTokenStore
{
    public static AccessTokenViewStorage Default { get; } = new();

    AccessTokenViewStorage() {}
}