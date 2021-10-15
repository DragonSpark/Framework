using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class Challenged<T> : IChallenged<T> where T : IdentityUser
{
	readonly IAuthentications<T> _authentications;
	readonly IAddLogin<T>        _add;

	public Challenged(IAuthentications<T> authentications, IAddLogin<T> add)
	{
		_authentications = authentications;
		_add             = add;
	}

	public async ValueTask<ChallengeResult<T>?> Get(ClaimsPrincipal parameter)
	{
		using var authentication = _authentications.Get();
		var       user           = await authentication.Users.GetUserAsync(parameter).ConfigureAwait(false);
		if (user != null)
		{
			var name = authentication.Users.GetUserId(parameter);
			var login = await authentication.Subject.GetExternalLoginInfoAsync(name).ConfigureAwait(false)
			            ?? throw new
				            InvalidOperationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
			var result = await _add.Await(new(login, user));
			return new(user, login, result);
		}

		return null;
	}
}