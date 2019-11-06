namespace DragonSpark.Compose.Conditions
{
	public sealed class DefaultExtent<T> : Extent<T>
	{
		public static DefaultExtent<T> Default { get; } = new DefaultExtent<T>();

		DefaultExtent() {}
	}
}