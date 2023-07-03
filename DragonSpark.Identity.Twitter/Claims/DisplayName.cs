using AspNet.Security.OAuth.Twitter;

namespace DragonSpark.Identity.Twitter.Claims;

public sealed class DisplayName : Text.Text
{
	public static DisplayName Default { get; } = new();

	DisplayName() : base(TwitterAuthenticationConstants.Claims.Name) {}
}