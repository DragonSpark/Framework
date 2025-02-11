using DragonSpark.Text;
using System.IdentityModel.Tokens.Jwt;

namespace DragonSpark.Application.Security.Identity;

public sealed class IdentityTokenParser : Parser<JwtSecurityToken>
{
	public static IdentityTokenParser Default { get; } = new();

	IdentityTokenParser() : this(TokenHandler.Default) {}

	public IdentityTokenParser(JwtSecurityTokenHandler handler) : base(handler.ReadJwtToken) {}
}