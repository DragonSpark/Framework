using DragonSpark.Application.Security.Identity.Bearer;
using DragonSpark.Composition;
using DragonSpark.Model.Results;
using DragonSpark.Model.Sequences;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class SecureTokenValidation : Instance<TokenValidationParameters>
{
    public SecureTokenValidation(BearerSettings settings) : this(settings, Convert.FromBase64String(settings.Key)) {}

    [Candidate(false)]
    public SecureTokenValidation(BearerSettings settings, Array<byte> key)
        : this(settings, new SymmetricSecurityKey(key)) {}

    [Candidate(false)]
    public SecureTokenValidation(BearerSettings settings, SecurityKey key)
        : base(new()
        {
            ValidateIssuer           = true, ValidIssuer   = settings.Issuer,
            ValidateAudience         = true, ValidAudience = settings.Audience,
            ValidateLifetime         = true, ClockSkew     = TimeSpan.FromSeconds(30),
            TokenDecryptionKey       = key,
            RequireSignedTokens      = false,
            ValidateIssuerSigningKey = false
        }) {}
}