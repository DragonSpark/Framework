namespace DragonSpark.Identity.Facebook.Claims
{
	public sealed class Picture : FacebookClaim
	{
		public static Picture Default { get; } = new Picture();

		Picture() : base(nameof(Picture).ToLowerInvariant()) {}
	}
}