namespace DragonSpark.Identity.DeviantArt.Claims
{
	public sealed class Description : DeviantArtClaim
	{
		public static Description Default { get; } = new Description();

		Description() : base(nameof(Description).ToLowerInvariant()) {}
	}
}