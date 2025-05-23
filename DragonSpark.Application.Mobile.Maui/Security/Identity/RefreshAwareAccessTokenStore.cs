using DragonSpark.Application.Communication.Http.Security;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class RefreshAwareAccessTokenStore
    : DragonSpark.Application.Communication.Http.Security.RefreshAwareAccessTokenStore
{
    public RefreshAwareAccessTokenStore(IComposeTokenView compose) : base(compose, AccessTokenViewStorage.Default) {}
}