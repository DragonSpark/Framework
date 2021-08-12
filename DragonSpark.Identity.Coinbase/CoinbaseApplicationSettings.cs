namespace DragonSpark.Identity.Coinbase
{
	public sealed class CoinbaseApplicationSettings
	{
		public string Key { get; set; }  = null!;

		public string Secret { get; set; }  = null!;

		public string[]? Scopes { get; set; }

	}
}