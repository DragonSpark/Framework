using System.Threading.Tasks;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Uno.Extensions.Authentication;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class AssignCurrentPrincipal : IOperation
{
	readonly ITokenCache      _tokens;
	readonly PrincipalStores  _stores;
	readonly IPrincipalAccess _store;

	public AssignCurrentPrincipal(ITokenCache tokens, IPrincipalAccess store)
		: this(tokens, PrincipalStores.Default, store) {}

	public AssignCurrentPrincipal(ITokenCache tokens, PrincipalStores stores, IPrincipalAccess store)
	{
		_tokens = tokens;
		_stores = stores;
		_store  = store;
	}

	public async ValueTask Get()
	{
		var access    = await _tokens.AccessTokenAsync().Off();
		var identity  = await _tokens.TokenAsync(TokenCacheExtensions.IdTokenKey).Off();
		var valid     = access.Account() is not null && identity.Account() is not null;
		var principal = valid ? _stores.Get(access).Get(identity) : AnonymousPrincipal.Default;
		_store.Execute(principal);
		if (!valid)
		{
			_stores.Execute();
		}
	}
}