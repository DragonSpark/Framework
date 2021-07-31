using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.DeviantArt.Claims
{
	public sealed class DescriptionClaimAction : SubKeyClaimAction
	{
		public static DescriptionClaimAction Default { get; } = new DescriptionClaimAction();

		DescriptionClaimAction() : base(Description.Default, "profile", "tagline") {}
	}
}