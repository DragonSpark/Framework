namespace DragonSpark.Presentation
{
	sealed class ProviderName : Text.Text
	{
		public static ProviderName Default { get; } = new ProviderName();

		ProviderName() : base("provider") {}
	}
}