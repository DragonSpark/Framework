using DragonSpark.Application.Security.Identity.Model;
using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

sealed class ClearCurrentAuthentication : ICommand<ClaimsPrincipal>
{
	readonly ICurrentPrincipal         _current;
	readonly IClearAuthenticationState _clear;

	public ClearCurrentAuthentication(ICurrentPrincipal current, IClearAuthenticationState clear)
	{
		_current = current;
		_clear   = clear;
	}

	public void Execute(ClaimsPrincipal parameter)
	{
		_clear.Execute(parameter);
		_clear.Execute(_current.Get());
	}
}

// TODO

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

sealed class AuthenticateExternal : IOperation<ExternalLoginInfo>
{
	readonly ICurrentContext _context;
	readonly string          _scheme;

	public AuthenticateExternal(ICurrentContext context) : this(context, IdentityConstants.ApplicationScheme) {}

	public AuthenticateExternal(ICurrentContext context, string scheme)
	{
		_context = context;
		_scheme  = scheme;
	}

	public ValueTask Get(ExternalLoginInfo parameter)
		=> _context.Get().SignInAsync(_scheme, parameter.Principal, parameter.AuthenticationProperties).ToOperation();
}