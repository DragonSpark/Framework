namespace DragonSpark.Identity.Google.Claims
{
	public class GoogleClaim : Text.Text
	{
		protected GoogleClaim(string name) : base($"{GoogleClaimNamespace.Default}:{name}") {}
	}
}