using DragonSpark.Model.Sequences;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace DragonSpark.Compose.Selections
{
	public sealed class SequenceExtent<T> : Extent<IEnumerable<T>>
	{
		public static SequenceExtent<T> Default { get; } = new SequenceExtent<T>();

		SequenceExtent() {}

		public Extent<T[]> Open => DefaultExtent<T[]>.Default;

		public Extent<IList<T>> List => DefaultExtent<IList<T>>.Default;

		public Extent<Array<T>> Array => DefaultExtent<Array<T>>.Default;

		public Extent<IReadOnlyList<T>> ReadOnly => DefaultExtent<IReadOnlyList<T>>.Default;

		public Extent<ImmutableArray<T>> Immutable => DefaultExtent<ImmutableArray<T>>.Default;
	}

	public sealed class SequenceExtent<TIn, TOut> : Extent<TIn, IEnumerable<TOut>>
	{
		public static SequenceExtent<TIn, TOut> Default { get; } = new SequenceExtent<TIn, TOut>();

		SequenceExtent() {}

		public Extent<TIn, TOut[]> Open => DefaultExtent<TIn, TOut[]>.Default;

		public Extent<TIn, Array<TOut>> Array => DefaultExtent<TIn, Array<TOut>>.Default;

		public Extent<TIn, ImmutableArray<TOut>> Immutable => DefaultExtent<TIn, ImmutableArray<TOut>>.Default;
	}
}