using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators
{
	sealed class Challenged<T> : IChallenged<T> where T : IdentityUser
	{
		readonly SignInManager<T> _authentication;
		readonly UserManager<T>   _users;
		readonly IAddLogin<T>     _add;

		public Challenged(SignInManager<T> authentication, IAddLogin<T> add)
			: this(authentication, authentication.UserManager, add) {}

		public Challenged(SignInManager<T> authentication, UserManager<T> users, IAddLogin<T> add)
		{
			_authentication = authentication;
			_users          = users;
			_add            = add;
		}

		public async ValueTask<ChallengeResult<T>?> Get(ClaimsPrincipal parameter)
		{
			var user = await _users.GetUserAsync(parameter).ConfigureAwait(false);
			if (user != null)
			{
				var name = _users.GetUserId(parameter);
				var login = await _authentication.GetExternalLoginInfoAsync(name).ConfigureAwait(false)
				            ?? throw new
					            InvalidOperationException($"Unexpected error occurred loading external login info for user with ID '{user.Id}'.");
				var result = await _add.Await(new(login, user));
				return new(user, login, result);
			}

			return null;
		}
	}
}