using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Commands
{
	public sealed class SequenceExtent<T> : Extent<IEnumerable<T>>
	{
		public static SequenceExtent<T> Default { get; } = new SequenceExtent<T>();

		SequenceExtent() {}

		public Extent<T[]> Array => DefaultExtent<T[]>.Default;

		public Extent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
	}
}