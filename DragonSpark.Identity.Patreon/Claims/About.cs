namespace DragonSpark.Identity.Patreon.Claims
{
	public sealed class About : PatreonClaim
	{
		public static About Default { get; } = new About();

		About() : base(nameof(About).ToLowerInvariant()) {}
	}
}
