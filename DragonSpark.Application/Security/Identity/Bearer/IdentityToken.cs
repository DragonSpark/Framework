using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class IdentityToken : ReferenceValueStore<SecurityTokenDescriptor, string>, IFormatter<SecurityTokenDescriptor>
{
	public static IdentityToken Default { get; } = new();

	IdentityToken() : this(TokenHandler.Default) {}

	public IdentityToken(JwtSecurityTokenHandler handler) : base(handler.CreateEncodedJwt) {}
}