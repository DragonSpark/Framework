namespace DragonSpark.Identity.DeviantArt.Claims;

public sealed class Image : DeviantArtClaim
{
	public static Image Default { get; } = new();

	Image() : base(nameof(Image).ToLowerInvariant()) {}
}