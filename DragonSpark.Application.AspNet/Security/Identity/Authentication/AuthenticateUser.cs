using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class AuthenticateUser<T> : IOperation<Login<T>>
{
	readonly IAuthenticate<T>           _authenticate;
	readonly ClearCurrentAuthentication _clear;

	public AuthenticateUser(IAuthenticate<T> authenticate, ClearCurrentAuthentication clear)
	{
		_authenticate = authenticate;
		_clear        = clear;
	}

	public async ValueTask Get(Login<T> parameter)
	{
		var (information, _) = parameter;
		await _authenticate.Await(parameter);
		_clear.Execute(information.Principal);
	}
}