using DragonSpark.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Sign : ISign
{
	readonly DetermineSecurityDescriptor         _descriptor;
	readonly IFormatter<SecurityTokenDescriptor> _token;

	public Sign(DetermineSecurityDescriptor descriptor) : this(descriptor, IdentityToken.Default) {}

	public Sign(DetermineSecurityDescriptor descriptor, IFormatter<SecurityTokenDescriptor> token)
	{
		_descriptor = descriptor;
		_token      = token;
	}

	public string Get(ClaimsIdentity parameter)
	{
		var descriptor = _descriptor.Get(parameter);
		var result     = _token.Get(descriptor);
		return result;
	}
}