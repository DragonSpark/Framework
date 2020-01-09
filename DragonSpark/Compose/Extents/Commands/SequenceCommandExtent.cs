using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Commands
{
	public sealed class SequenceCommandExtent<T> : CommandExtent<IEnumerable<T>>
	{
		public static SequenceCommandExtent<T> Default { get; } = new SequenceCommandExtent<T>();

		SequenceCommandExtent() {}

		public CommandExtent<T[]> Array => DefaultCommandExtent<T[]>.Default;

		public CommandExtent<Array<T>> Immutable => DefaultCommandExtent<Array<T>>.Default;
	}
}