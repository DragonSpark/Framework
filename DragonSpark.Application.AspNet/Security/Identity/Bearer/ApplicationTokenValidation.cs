using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using DragonSpark.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class ApplicationTokenValidation : Instance<TokenValidationParameters>
{
    public ApplicationTokenValidation(BearerSettings settings)
        : this(settings, EncodedTextAsData.Default.Get(settings.Key)) {}

    [Candidate(false)]
    public ApplicationTokenValidation(BearerSettings settings, byte[] key)
        : this(settings, new SymmetricSecurityKey(key)) {}

    [Candidate(false)]
    public ApplicationTokenValidation(BearerSettings settings, SecurityKey key)
        : base(new()
        {
            ValidateIssuer           = true,
            ValidateAudience         = true,
            ValidateLifetime         = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer              = settings.Issuer,
            ValidAudience            = settings.Audience,
            AuthenticationType       = IdentityConstants.ApplicationScheme,
            IssuerSigningKey         = key
        }) {}
}