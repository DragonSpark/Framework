using DragonSpark.Model.Selection.Stores;
using DragonSpark.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class IdentityTokenFormatter : ReferenceValueStore<SecurityTokenDescriptor, string>,
                                      IFormatter<SecurityTokenDescriptor>
{
	public static IdentityTokenFormatter Default { get; } = new();

	IdentityTokenFormatter() : this(TokenHandler.Default) {}

	public IdentityTokenFormatter(JwtSecurityTokenHandler handler) : base(handler.CreateEncodedJwt) {}
}