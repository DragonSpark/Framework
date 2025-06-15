using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Application.AspNet.Security.Identity.Profile;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

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

	public async ValueTask<ChallengeResult<T>?> Get(Stop<ClaimsPrincipal> parameter)
	{
		var previous = await _previous.Off(parameter);

		if (previous.HasValue)
		{
			var (user, information, result) = previous.Value;
			if (result.Succeeded)
			{
				await _authenticate.Off(new(new(information, user), parameter));
				await _synchronization.Off(new(information, parameter));
			}
		}

		return previous;
	}
}