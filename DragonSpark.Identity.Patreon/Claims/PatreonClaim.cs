namespace DragonSpark.Identity.Patreon.Claims
{
	public class PatreonClaim : Text.Text
	{
		protected PatreonClaim(string name) : base($"{PatreonClaimNamespace.Default}:{name}") {}
	}
}