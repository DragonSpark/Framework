using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences.Collections;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Query;

class SortAlteration<T> : IAlteration<T[]>
{
	public static SortAlteration<T> Default { get; } = new SortAlteration<T>();

	SortAlteration() : this(SortComparer<T>.Default) {}

	readonly IComparer<T> _comparer;

	public SortAlteration(IComparer<T> comparer) => _comparer = comparer;

	public T[] Get(T[] parameter)
	{
		System.Array.Sort(parameter, _comparer);
		return parameter;
	}
}