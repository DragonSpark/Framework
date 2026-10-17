using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Entra;

sealed class EntraAwareLocateUser<T> : ILocateUser<T> where T : IdentityUser
{
	readonly ILocateUser<T> _previous;
	readonly IUsers<T>      _users;

	public EntraAwareLocateUser(ILocateUser<T> previous, IUsers<T> users)
	{
		_previous = previous;
		_users    = users;
	}

	public async ValueTask<T?> Get(ExternalLoginInfo parameter)
	{
		var previous = await _previous.Off(parameter);

		if (previous is null && parameter.Principal.Email() is {} email)
		{
			using var users  = _users.Get();
			var       result = await users.Subject.FindByEmailAsync(email).Off();
			return result;
		}

		return previous;
	}
}