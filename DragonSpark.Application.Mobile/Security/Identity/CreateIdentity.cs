using DragonSpark.Application.Security.Identity;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DragonSpark.Application.Mobile.Security.Identity;

sealed class CreateIdentity : ISelect<string, ClaimsIdentity>
{
	public static CreateIdentity Default { get; } = new();

	CreateIdentity() : this(IdentityTokenParser.Default) {}

	readonly IParser<JwtSecurityToken> _token;

	public CreateIdentity(IParser<JwtSecurityToken> token) => _token = token;

	public ClaimsIdentity Get(string parameter) => new(_token.Get(parameter).Claims, JwtConstants.HeaderType);
}