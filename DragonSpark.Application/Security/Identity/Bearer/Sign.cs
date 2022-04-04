using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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

sealed class IdentityToken : ReferenceValueStore<SecurityTokenDescriptor, string>, IFormatter<SecurityTokenDescriptor>
{
	public static IdentityToken Default { get; } = new();

	IdentityToken() : this(TokenHandler.Default) {}

	public IdentityToken(JwtSecurityTokenHandler handler) : base(handler.CreateEncodedJwt) {}
}

sealed class TokenHandler : Instance<JwtSecurityTokenHandler>
{
	public static TokenHandler Default { get; } = new();

	TokenHandler() : base(new JwtSecurityTokenHandler()) {}
}