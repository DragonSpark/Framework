using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class RefreshUser<T> : IRefreshUser where T : IdentityUser
{
	readonly IAuthentications<T>       _authentications;
	readonly IClearAuthenticationState _clear;
	readonly IRefreshAuthentication<T> _refresh;

	public RefreshUser(IAuthentications<T> authentications, IClearAuthenticationState clear,
	                   IRefreshAuthentication<T> refresh)
	{
		_authentications = authentications;
		_clear           = clear;
		_refresh         = refresh;
	}

	public async ValueTask Get(ClaimsPrincipal parameter)
	{
		using var authentication = _authentications.Get();
		if (authentication.Subject.IsSignedIn(parameter))
		{
			_clear.Execute(parameter);
			var user = await authentication.Users.FindByIdAsync(authentication.Users.GetUserId(parameter).Verify())
			                               .ConfigureAwait(false);
			await _refresh.Await(user.Verify());
		}
	}
}