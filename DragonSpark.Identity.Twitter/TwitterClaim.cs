namespace DragonSpark.Identity.Twitter {
	public class TwitterClaim : Text.Text
	{
		protected TwitterClaim(string name) : base($"urn:twitter:{name}") {}
	}
}