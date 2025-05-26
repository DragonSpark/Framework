using DragonSpark.Application.Communication.Http.Security;

namespace DragonSpark.Application.AspNet.Communication.Http;

sealed class AccessTokenStore : DragonSpark.Model.Operations.Results.Stop.Storing<AccessTokenView?>, IAccessTokenStore
{
	public AccessTokenStore(AccessTokenStorage store, ComposeAccessTokenView refresh) : base(store, refresh) {}
}