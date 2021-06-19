namespace DragonSpark.Application.Security.Identity.Claims
{
	public sealed class ExtractClaims : ClaimExtractor
	{
		public ExtractClaims(IKnownClaims names) : base(new ClaimNames(names)) {}
	}
}