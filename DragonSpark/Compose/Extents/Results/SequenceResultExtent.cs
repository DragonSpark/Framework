using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Results
{
	public sealed class SequenceResultExtent<T> : ResultExtent<IEnumerable<T>>
	{
		public static SequenceResultExtent<T> Default { get; } = new SequenceResultExtent<T>();

		SequenceResultExtent() {}

		public ArrayResultExtent<T> Array => ArrayResultExtent<T>.Default;

		public ResultExtent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
	}
}