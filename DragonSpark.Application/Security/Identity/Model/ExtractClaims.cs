namespace DragonSpark.Application.Security.Identity.Model
{
	public sealed class ExtractClaims : ClaimExtractor
	{
		public ExtractClaims(IKnownClaims names) : base(new ClaimNames(names)) {}
	}
}