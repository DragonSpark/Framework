namespace DragonSpark.Identity.Facebook.Claims
{
	public class FacebookClaim : Text.Text
	{
		protected FacebookClaim(string name) : base($"{FacebookClaimNamespace.Default}:{name}") {}
	}
}