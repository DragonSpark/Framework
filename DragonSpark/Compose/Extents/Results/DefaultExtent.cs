namespace DragonSpark.Compose.Extents.Results
{
	public sealed class DefaultExtent<T> : ResultExtent<T>
	{
		public static DefaultExtent<T> Default { get; } = new DefaultExtent<T>();

		DefaultExtent() {}
	}
}