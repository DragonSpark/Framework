namespace DragonSpark.Identity.Google.Claims
{
	public sealed class Picture : GoogleClaim
	{
		public static Picture Default { get; } = new Picture();

		Picture() : base(nameof(Picture).ToLowerInvariant()) {}
	}
}