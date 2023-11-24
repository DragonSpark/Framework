namespace DragonSpark.Identity.DeviantArt.Claims;

public sealed class Description : DeviantArtClaim
{
	public static Description Default { get; } = new();

	Description() : base(nameof(Description).ToLowerInvariant()) {}
}