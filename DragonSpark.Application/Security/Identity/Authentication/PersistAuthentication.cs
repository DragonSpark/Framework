using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class PersistAuthentication<T> : IOperation<StoreAuthenticationInput<T>> where T : IdentityUser
{
	readonly IAuthentications<T> _authentication;
	readonly LogAuthentication   _log;
	readonly bool                _persist;

	public PersistAuthentication(IAuthentications<T> authentication, LogAuthentication log, bool persist = true)
	{
		_authentication = authentication;
		_log            = log;
		_persist        = persist;
	}

	public async ValueTask Get(StoreAuthenticationInput<T> parameter)
	{
		var (user, claims) = parameter;

		using var authentication = _authentication.Get();
		await authentication.Subject.SignInWithClaimsAsync(user, _persist, claims.Open()).ConfigureAwait(false);
		_log.Execute(new(user.UserName, claims));
	}
}