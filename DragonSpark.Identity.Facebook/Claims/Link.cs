namespace DragonSpark.Identity.Facebook.Claims;

public sealed class Link : FacebookClaim
{
	public static Link Default { get; } = new();

	Link() : base(nameof(Link).ToLowerInvariant()) {}
}