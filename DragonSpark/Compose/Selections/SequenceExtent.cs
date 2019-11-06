using System.Collections.Generic;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Compose.Selections
{
	public sealed class SequenceExtent<T> : Extent<IEnumerable<T>>
	{
		public static SequenceExtent<T> Default { get; } = new SequenceExtent<T>();

		SequenceExtent() {}

		public Extent<T[]> Array => DefaultExtent<T[]>.Default;

		public Extent<IList<T>> List => DefaultExtent<IList<T>>.Default;

		public Extent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
	}

	public sealed class SequenceExtent<TIn, TOut> : Extent<TIn, IEnumerable<TOut>>
	{
		public static SequenceExtent<TIn, TOut> Default { get; } = new SequenceExtent<TIn, TOut>();

		SequenceExtent() {}

		public Extent<TIn, TOut[]> Array => DefaultExtent<TIn, TOut[]>.Default;

		public Extent<TIn, Array<TOut>> Immutable => DefaultExtent<TIn, Array<TOut>>.Default;
	}
}