using DragonSpark.Application.Communication.Http;
using DragonSpark.Application.Communication.Http.Security;

namespace DragonSpark.Application.Mobile.Maui.Security.Identity;

sealed class AccessTokenProvider : DragonSpark.Application.Communication.Http.Security.AccessTokenProvider
{
    public AccessTokenProvider(IAccessTokenProvider previous, IAccessTokenStore store) : base(previous, store) {}
}