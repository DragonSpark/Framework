using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Conditions
{
	public sealed class ConditionSequenceExtent<T> : ConditionExtent<IEnumerable<T>>
	{
		public static ConditionSequenceExtent<T> Default { get; } = new ConditionSequenceExtent<T>();

		ConditionSequenceExtent() {}

		public ConditionExtent<T[]> Array => DefaultConditionExtent<T[]>.Default;

		public ConditionExtent<Array<T>> Immutable => DefaultConditionExtent<Array<T>>.Default;
	}
}