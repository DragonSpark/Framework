namespace DragonSpark.Compose
{
	public sealed class VowelContext
	{
		public static VowelContext Default { get; } = new VowelContext();

		VowelContext() : this(Extents.Extents.Default) {}

		public VowelContext(Extents.Extents extent) => Extent = extent;

		public Extents.Extents Extent { get; }
	}
}