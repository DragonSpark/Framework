using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class LocationClaimAction : SubKeyClaimAction
{
	public static LocationClaimAction Default { get; } = new();

	LocationClaimAction() : base(Location.Default, "data", "location") {}
}