using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class LocationClaimAction : ClaimAction
	{
		public static LocationClaimAction Default { get; } = new LocationClaimAction();

		LocationClaimAction() : base(Location.Default, "location") {}
	}
}