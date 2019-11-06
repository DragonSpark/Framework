namespace DragonSpark.Compose.Extents
{
	public sealed class Extents
	{
		public static Extents Default { get; } = new Extents();

		Extents() {}

		public Extent Of => Extent.Default;
	}
}