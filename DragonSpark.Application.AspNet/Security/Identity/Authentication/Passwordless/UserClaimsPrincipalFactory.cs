using System.Security.Claims;
using DragonSpark.Compose;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DragonSpark.Application.AspNet.Security.Identity.Authentication.Passwordless;

public class UserClaimsPrincipalFactory<T> : Microsoft.AspNetCore.Identity.UserClaimsPrincipalFactory<T>
    where T : class
{
    readonly IComposeClaims<T> _claims;

    protected UserClaimsPrincipalFactory(UserManager<T> userManager, IOptions<IdentityOptions> optionsAccessor,
                                         IComposeClaims<T> claims)
        : base(userManager, optionsAccessor)
        => _claims = claims;

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(T user)
    {
        var result = await base.GenerateClaimsAsync(user).Off();
        result.AddClaims(_claims.Get(user));
        return result;
    }
}