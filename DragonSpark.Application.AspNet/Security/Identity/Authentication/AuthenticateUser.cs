using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Stop;
using System.Threading.Tasks;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication;

sealed class AuthenticateUser<T> : IStopAware<Login<T>>
{
	readonly IAuthenticate<T>           _authenticate;
	readonly ClearCurrentAuthentication _clear;

	public AuthenticateUser(IAuthenticate<T> authenticate, ClearCurrentAuthentication clear)
	{
		_authenticate = authenticate;
		_clear        = clear;
	}

	public async ValueTask Get(Stop<Login<T>> parameter)
	{
		var ((information, _), _) = parameter;
		await _authenticate.Off(parameter);
		_clear.Execute(information.Principal);
	}
}