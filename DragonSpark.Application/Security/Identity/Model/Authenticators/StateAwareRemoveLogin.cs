using DragonSpark.Application.Security.Identity.Authentication;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Model.Authenticators;

sealed class StateAwareRemoveLogin<T> : IRemoveLogin<T> where T : class
{
	readonly IRemoveLogin<T>           _previous;
	readonly IAuthentications<T>       _authentications;
	readonly IClearAuthenticationState _clear;

	public StateAwareRemoveLogin(IRemoveLogin<T> previous, IAuthentications<T> authentications,
	                             IClearAuthenticationState clear)
	{
		_previous        = previous;
		_authentications = authentications;
		_clear           = clear;
	}

	public async ValueTask<IdentityResult> Get(RemoveLoginInput<T> parameter)
	{
		var result = await _previous.Await(parameter);
		if (result.Succeeded)
		{
			var (user, _) = parameter;
			using var authentications = _authentications.Get();
			var       principal       = await authentications.Subject.CreateUserPrincipalAsync(user);
			_clear.Execute(principal);
		}

		return result;
	}
}