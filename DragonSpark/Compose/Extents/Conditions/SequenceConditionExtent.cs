using DragonSpark.Model.Sequences;
using System.Collections.Generic;

namespace DragonSpark.Compose.Extents.Conditions;

public sealed class SequenceConditionExtent<T> : ConditionExtent<IEnumerable<T>>
{
	public static SequenceConditionExtent<T> Default { get; } = new SequenceConditionExtent<T>();

	SequenceConditionExtent() {}

	public ConditionExtent<T[]> Array => DefaultConditionExtent<T[]>.Default;

	public ConditionExtent<Array<T>> Immutable => DefaultConditionExtent<Array<T>>.Default;
}