using DragonSpark.Model.Results;
using System.IdentityModel.Tokens.Jwt;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class TokenHandler : Instance<JwtSecurityTokenHandler>
{
	public static TokenHandler Default { get; } = new();

	TokenHandler() : base(new JwtSecurityTokenHandler()) {}
}