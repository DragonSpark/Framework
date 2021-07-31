namespace DragonSpark.Identity.DeviantArt.Claims
{
	public class DeviantArtClaim : Text.Text
	{
		protected DeviantArtClaim(string name) : base($"{DeviantArtClaimNamespace.Default}:{name}") {}
	}
}