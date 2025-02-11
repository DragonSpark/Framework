using DragonSpark.Text;
using System.IdentityModel.Tokens.Jwt;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class IdentityTokenParser : Parser<JwtSecurityToken>
{
	public static IdentityTokenParser Default { get; } = new();

	IdentityTokenParser() : this(TokenHandler.Default) {}

	public IdentityTokenParser(JwtSecurityTokenHandler handler) : base(handler.ReadJwtToken) {}
}