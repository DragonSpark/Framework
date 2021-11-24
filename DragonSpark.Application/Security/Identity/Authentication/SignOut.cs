using DragonSpark.Application.Security.Identity.Model;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

public sealed class SignOut<T> : ISignOut where T : class
{
	readonly IAuthentications<T>       _authentications;
	readonly IClearAuthenticationState _state;

	public SignOut(IAuthentications<T> authentications, IClearAuthenticationState state)
	{
		_authentications = authentications;
		_state           = state;
	}

	public async ValueTask Get(ClaimsPrincipal parameter)
	{
		using var authentication = _authentications.Get();
		if (authentication.Subject.IsSignedIn(parameter))
		{
			_state.Execute(parameter);
			await authentication.Subject.SignOutAsync().ConfigureAwait(false);
		}
	}
}