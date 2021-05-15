namespace DragonSpark.Identity.Coinbase
{
	public sealed class CoinbaseAuthenticationSettings
	{
		public string[]? Scopes { get; set; }

		public string AuthorizationEndpoint { get; set; } = "https://www.coinbase.com/oauth/authorize";
		public string TokenEndpoint { get; set; } = "https://api.coinbase.com/oauth/token";
	}
}