namespace DragonSpark.Compose
{
	public interface IVowelContext
	{
		Extents.Extents Extent { get; }
	}

	public sealed class VowelContext : IVowelContext
	{
		public static VowelContext Default { get; } = new VowelContext();

		VowelContext() {}

		public Extents.Extents Extent => Extents.Extents.Default;
	}
}