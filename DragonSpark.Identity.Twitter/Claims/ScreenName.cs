using System.Security.Claims;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class ScreenName : Text.Text
{
	public static ScreenName Default { get; } = new();

	ScreenName() : base(ClaimTypes.Name) {}
}