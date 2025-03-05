using System.Threading.Tasks;
using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.AspNet.Security.Identity.Model.Authenticators;

sealed class StateAwareRemoveLogin<T>(
	IRemoveLogin<T> previous,
	IAuthentications<T> authentications,
	IClearAuthenticationState clear)
	: IRemoveLogin<T>
	where T : class
{
	readonly IRemoveLogin<T>           _previous        = previous;
	readonly IAuthentications<T>       _authentications = authentications;
	readonly IClearAuthenticationState _clear           = clear;

	public async ValueTask<IdentityResult> Get(RemoveLoginInput<T> parameter)
	{
		var result = await _previous.Await(parameter);
		if (result.Succeeded)
		{
			var (user, _) = parameter;
			using var authentications = _authentications.Get();
			var       principal = await authentications.Subject.CreateUserPrincipalAsync(user).Go();
			var       number = principal.Number();
			if (number is not null)
			{
				_clear.Execute(number.Value);
			}
		}

		return result;
	}
}
