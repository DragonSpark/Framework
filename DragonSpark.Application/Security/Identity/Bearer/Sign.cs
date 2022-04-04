using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class Sign : ISign
{
	readonly GetSecurityDescriptor   _descriptor;
	readonly JwtSecurityTokenHandler _handler;

	public Sign(GetSecurityDescriptor descriptor) : this(descriptor, new JwtSecurityTokenHandler()) {}

	public Sign(GetSecurityDescriptor descriptor, JwtSecurityTokenHandler handler)
	{
		_descriptor = descriptor;
		_handler    = handler;
	}

	public string Get(ClaimsIdentity parameter)
	{
		var descriptor = _descriptor.Get(parameter);
		var result     = _handler.CreateEncodedJwt(descriptor);
		return result;
	}
}