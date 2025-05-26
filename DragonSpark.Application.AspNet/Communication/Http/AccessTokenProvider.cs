using DragonSpark.Application.Communication.Http;
using DragonSpark.Application.Communication.Http.Security;

namespace DragonSpark.Application.AspNet.Communication.Http;

sealed class AccessTokenProvider : Application.Communication.Http.Security.AccessTokenProvider
{
	public AccessTokenProvider(IAccessTokenProvider previous, IAccessTokenStore view) : base(previous, view) {}
}