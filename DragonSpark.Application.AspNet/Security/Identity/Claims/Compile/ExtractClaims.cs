namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Compile;

sealed class ExtractClaims : ClaimExtractor
{
	public ExtractClaims(IKnownClaims names) : base(new ClaimNames(names)) {}
}