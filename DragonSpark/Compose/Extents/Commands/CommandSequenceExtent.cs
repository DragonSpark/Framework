using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Commands
{
	public sealed class CommandSequenceExtent<T> : CommandExtent<IEnumerable<T>>
	{
		public static CommandSequenceExtent<T> Default { get; } = new CommandSequenceExtent<T>();

		CommandSequenceExtent() {}

		public CommandExtent<T[]> Array => DefaultCommandExtent<T[]>.Default;

		public CommandExtent<Array<T>> Immutable => DefaultCommandExtent<Array<T>>.Default;
	}
}