using DragonSpark.Model.Results;
using DragonSpark.Text;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class BearerSigningCredentials : Instance<SigningCredentials>
{
	public BearerSigningCredentials(BearerSettings settings)
		: this(new SymmetricSecurityKey(EncodedTextAsData.Default.Get(settings.Key))) {}

	BearerSigningCredentials(SecurityKey key)
		: base(new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)) {}
}