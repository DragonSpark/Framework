using System.Security.Claims;
using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Application.Security.Identity.Profile;

sealed class Identifier : RequiredClaim
{
	public static Identifier Default { get; } = new();

	Identifier() : base(ClaimTypes.NameIdentifier) {}
}

sealed class UserName : RequiredClaim
{
    public static UserName Default { get; } = new();

    UserName() : base(ClaimTypes.Name) {}
}

//

sealed class Address : RequiredClaim
{
    public static Address Default { get; } = new();

    Address() : base(ClaimTypes.Name) {}
}