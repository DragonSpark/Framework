using DragonSpark.Application.AspNet.Security.Identity.Authentication;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

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

	public async ValueTask<IdentityResult> Get(Stop<RemoveLoginInput<T>> parameter)
	{
		var result = await _previous.Off(parameter);
		if (result.Succeeded)
		{
			var ((user, _), _) = parameter;
			using var authentications = _authentications.Get();
			var       principal = await authentications.Subject.CreateUserPrincipalAsync(user).On();
			var       number = principal.Number();
			if (number is not null)
			{
				_clear.Execute(number.Value);
			}
		}

		return result;
	}
}
