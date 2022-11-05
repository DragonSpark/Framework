using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DragonSpark.Model.Sequences.Collections;

public class Contains<T> : ICondition<T>
{
	readonly ICollection<T>       _source;
	readonly IEqualityComparer<T> _comparer;

	protected Contains(IEqualityComparer<T> comparer, params T[] source) : this(source, comparer) {}

	public Contains(ICollection<T> source, IEqualityComparer<T> comparer)
	{
		_source   = source;
		_comparer = comparer;
	}

	public bool Get(T parameter) => _source.Contains(parameter, _comparer);
}

public class Contains : Contains<string>
{
	protected Contains(params string[] source) : base(StringComparer.InvariantCultureIgnoreCase, source) {}
}