using AspNet.Security.OAuth.Reddit;

namespace DragonSpark.Identity.Reddit
{
	public sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base(RedditAuthenticationDefaults.AuthenticationScheme) {}
	}
}