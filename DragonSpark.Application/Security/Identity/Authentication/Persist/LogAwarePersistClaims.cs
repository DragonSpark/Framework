using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication.Persist;

sealed class LogAwarePersistClaims<T> : IPersistClaims<T> where T : IdentityUser
{
	readonly IPersistClaims<T> _previous;
	readonly LogClaims         _log;

	public LogAwarePersistClaims(IPersistClaims<T> previous, LogClaims log)
	{
		_previous = previous;
		_log      = log;
	}

	public async ValueTask Get(Claims<T> parameter)
	{
		var (user, claims) = parameter;
		await _previous.Await(parameter);
		_log.Execute(new(user.UserName, claims));
	}
}