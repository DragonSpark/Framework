namespace DragonSpark.Identity.Coinbase
{
	public sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base("Coinbase") {}
	}
}