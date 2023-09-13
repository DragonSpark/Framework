using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace DragonSpark.Application.Security.Identity.Authentication;

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