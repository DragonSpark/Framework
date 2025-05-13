using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using DragonSpark.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class TokenValidation : Instance<TokenValidationParameters>
{
	public TokenValidation(BearerSettings settings) : this(settings, EncodedTextAsData.Default.Get(settings.Key)) {}

	[Candidate(false)]
	public TokenValidation(BearerSettings settings, byte[] key)
		: base(new()
		{
			ValidateIssuer           = true,
			ValidateAudience         = true,
			ValidateLifetime         = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer              = settings.Issuer,
			ValidAudience            = settings.Audience,
			AuthenticationType       = IdentityConstants.ApplicationScheme,
			IssuerSigningKey         = new SymmetricSecurityKey(key)
		}) {}
}