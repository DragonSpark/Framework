using DragonSpark.Application.AspNet.Security.Identity.Model;
using DragonSpark.Compose;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

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
		var       number         = parameter.Number();
		if (number is not null)
		{
			_state.Execute(number.Value);
		}

		await authentication.Subject.SignOutAsync().Off();
	}
}