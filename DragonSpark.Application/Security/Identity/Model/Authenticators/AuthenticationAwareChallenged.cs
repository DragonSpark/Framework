using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Application.Security.Identity.Profile;
using DragonSpark.Compose;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class AuthenticationAwareChallenged<T> : IChallenged<T> where T : IdentityUser
{
	readonly IChallenged<T>       _previous;
	readonly IAuthenticate<T>     _authenticate;
	readonly IUserSynchronization _synchronization;

	public AuthenticationAwareChallenged(IChallenged<T> previous, IAuthenticate<T> authenticate,
	                                     IUserSynchronization synchronization)
	{
		_previous        = previous;
		_authenticate    = authenticate;
		_synchronization = synchronization;
	}

	public async ValueTask<ChallengeResult<T>?> Get(ClaimsPrincipal parameter)
	{
		var previous = await _previous.Await(parameter);

		if (previous.HasValue)
		{
			var (user, information, result) = previous.Value;
			if (result.Succeeded)
			{
				await _authenticate.Await(new(information, user));
				await _synchronization.Await(information);
			}
		}

		return previous;
	}
}