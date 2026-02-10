using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.Security.Identity.Bearer;

public sealed class BearerSigningCredentials : BearerSigningCredentialsBase
{
    public BearerSigningCredentials(BearerSettings settings) : base(settings, SecurityAlgorithms.HmacSha256Signature) {}
}