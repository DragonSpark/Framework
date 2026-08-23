using DragonSpark.Application.AspNet.Security.Identity;
using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityUser = DragonSpark.Application.AspNet.Security.Identity.IdentityUser;

namespace DragonSpark.Entra;

sealed class EntraAwareLocateUser<T> : ILocateUser<T> where T : IdentityUser
{
	readonly ILocateUser<T> _previous;
	readonly IUsers<T>      _users;
	readonly string         _claim;

	public EntraAwareLocateUser(ILocateUser<T> previous, IUsers<T> users)
		: this(previous, users, EmailClaim.Default) {}

	public EntraAwareLocateUser(ILocateUser<T> previous, IUsers<T> users, string claim)
	{
		_previous = previous;
		_users    = users;
		_claim    = claim;
	}

	public async ValueTask<T?> Get(ExternalLoginInfo parameter)
	{
		var result = await _previous.Off(parameter);

		if (result is null && parameter.Principal.FindFirstValue(_claim) is {} email)
		{
			using var users = _users.Get();
			return await users.Subject.FindByEmailAsync(email).Off();
		}

		return result;
	}
}