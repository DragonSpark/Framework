using DragonSpark.Application.Security.Identity.Claims.Actions;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class DescriptionClaimAction : SubKeyClaimAction
{
	public static DescriptionClaimAction Default { get; } = new();

	DescriptionClaimAction() : base(Description.Default, "data", "description") {}
}