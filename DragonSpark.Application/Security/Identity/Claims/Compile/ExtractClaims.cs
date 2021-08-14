namespace DragonSpark.Application.Security.Identity.Claims.Compile
{
	public sealed class ExtractClaims : ClaimExtractor
	{
		public ExtractClaims(IKnownClaims names) : base(new ClaimNames(names)) {}
	}
}