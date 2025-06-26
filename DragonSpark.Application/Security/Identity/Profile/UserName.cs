using System.Security.Claims;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class UserName : RequiredClaim
{
    public static UserName Default { get; } = new();

    UserName() : base(ClaimTypes.Name) {}
}