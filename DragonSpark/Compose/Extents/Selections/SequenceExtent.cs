using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Compose.Extents.Selections;

public sealed class SequenceExtent<T> : SelectionExtent<IEnumerable<T>>
{
	public static SequenceExtent<T> Default { get; } = new SequenceExtent<T>();

	SequenceExtent() {}

	public SelectionExtent<T[]> Open => DefaultSelectionExtent<T[]>.Default;

	public SelectionExtent<IList<T>> List => DefaultSelectionExtent<IList<T>>.Default;

	public SelectionExtent<Array<T>> Array => DefaultSelectionExtent<Array<T>>.Default;

	public SelectionExtent<ImmutableArray<T>> Immutable => DefaultSelectionExtent<ImmutableArray<T>>.Default;
}

public sealed class SequenceExtent<TIn, TOut> : SelectionExtent<TIn, IEnumerable<TOut>>
{
	public static SequenceExtent<TIn, TOut> Default { get; } = new SequenceExtent<TIn, TOut>();

	SequenceExtent() {}

	public SelectionExtent<TIn, TOut[]> Open => DefaultSelectionExtent<TIn, TOut[]>.Default;

	public SelectionExtent<TIn, Array<TOut>> Array => DefaultSelectionExtent<TIn, Array<TOut>>.Default;

	public SelectionExtent<TIn, ImmutableArray<TOut>> Immutable
		=> DefaultSelectionExtent<TIn, ImmutableArray<TOut>>.Default;
}