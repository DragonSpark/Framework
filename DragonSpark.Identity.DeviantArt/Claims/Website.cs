namespace DragonSpark.Identity.DeviantArt.Claims
{
	public sealed class Website : DeviantArtClaim
	{
		public static Website Default { get; } = new ();

		Website() : base(nameof(Website).ToLower()) {}
	}
}