using DragonSpark.Application.AspNet.Security.Identity.Model;
using DragonSpark.Compose;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

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
			var number = parameter.Number();
			if (number is not null)
			{
				_clear.Execute(number.Value);
			}
			var user = await authentication.Users.FindByIdAsync(authentication.Users.GetUserId(parameter).Verify())
			                               .Off();
			await _refresh.Off(user.Verify());
		}
	}
}