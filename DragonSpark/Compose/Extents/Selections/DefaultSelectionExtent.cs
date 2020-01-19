namespace DragonSpark.Compose.Extents.Selections
{
	public sealed class DefaultSelectionExtent<T> : SelectionExtent<T>
	{
		public static DefaultSelectionExtent<T> Default { get; } = new DefaultSelectionExtent<T>();

		DefaultSelectionExtent() {}
	}

	public sealed class DefaultSelectionExtent<TIn, TOut> : SelectionExtent<TIn, TOut>
	{
		public static DefaultSelectionExtent<TIn, TOut> Default { get; } = new DefaultSelectionExtent<TIn, TOut>();

		DefaultSelectionExtent() {}
	}
}