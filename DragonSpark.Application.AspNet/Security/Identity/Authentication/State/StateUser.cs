using DragonSpark.Compose;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.State;

sealed class StateUser<T> : IStateUser<T> where T : IdentityUser
{
	readonly IUsers<T>             _users;
	readonly ILogger<StateUser<T>> _log;

	[UsedImplicitly]
	public StateUser(IUsers<T> users, ILogger<StateUser<T>> log)
	{
		_users = users;
		_log   = log;
	}

	public async ValueTask<T?> Get(ClaimsPrincipal parameter)
	{
		try
		{
			using var users  = _users.Get();
			var       result = await users.Subject.GetUserAsync(parameter).Off();
			return result;
		}
		catch (Exception e)
		{
			_log.LogWarning(e, "There was a problem parsing the user's identity {Identity}",
			                parameter.FindFirstValue(ClaimTypes.NameIdentifier));
			return null;
		}
	}
}