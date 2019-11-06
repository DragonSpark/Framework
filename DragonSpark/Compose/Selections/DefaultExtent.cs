namespace DragonSpark.Compose.Selections
{
	public sealed class DefaultExtent<T> : Extent<T>
	{
		public static DefaultExtent<T> Default { get; } = new DefaultExtent<T>();

		DefaultExtent() {}
	}

	public sealed class DefaultExtent<TIn, TOut> : Extent<TIn, TOut>
	{
		public static DefaultExtent<TIn, TOut> Default { get; } = new DefaultExtent<TIn, TOut>();

		DefaultExtent() {}
	}
}