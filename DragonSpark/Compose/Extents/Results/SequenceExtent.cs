using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Results
{
	public sealed class SequenceExtent<T> : Extent<IEnumerable<T>>
	{
		public static SequenceExtent<T> Default { get; } = new SequenceExtent<T>();

		SequenceExtent() {}

		public ArrayExtent<T> Array => ArrayExtent<T>.Default;

		public Extent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
	}
}