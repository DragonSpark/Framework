namespace DragonSpark.Identity.Reddit.Claims;

public sealed class Image : RedditClaim
{
	public static Image Default { get; } = new();

	Image() : base(nameof(Image).ToLowerInvariant()) {}
}