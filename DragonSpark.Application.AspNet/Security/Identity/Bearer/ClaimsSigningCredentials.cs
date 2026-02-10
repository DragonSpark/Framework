using DragonSpark.Application.Security.Identity.Bearer;
using Microsoft.IdentityModel.Tokens;

namespace DragonSpark.Application.AspNet.Security.Identity.Bearer;

public sealed class ClaimsSigningCredentials : BearerSigningCredentialsBase
{
    public ClaimsSigningCredentials(BearerSettings settings) : base(settings, SecurityAlgorithms.HmacSha256) {}
}