using DragonSpark.Application.Security.Identity.Claims;

namespace DragonSpark.Identity.Twitter.Claims
{
	public sealed class DescriptionClaimAction : ClaimAction
	{
		public static DescriptionClaimAction Default { get; } = new DescriptionClaimAction();

		DescriptionClaimAction() : base(Description.Default, "description") {}
	}
}