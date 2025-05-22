using DragonSpark.Application.Communication.Http;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class AccessTokenProvider : DragonSpark.Application.Communication.Http.Security.AccessTokenProvider
{
    public AccessTokenProvider(IAccessTokenProvider previous) : base(previous, AccessTokenStore.Default) {}
}