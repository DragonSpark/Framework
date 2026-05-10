using DragonSpark.Model.Selection;
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

// TODO V2:


public readonly record struct CastInput(object[] Input, Type To);

public sealed class Cast : ISelect<CastInput, System.Array>
{
	public static Cast Default { get; } = new();

	Cast() {}
	
	public System.Array Get(CastInput parameter)
	{
		var (input, to) = parameter;
		var result       = System.Array.CreateInstance(to, input.Length);

		for (var i = 0; i < input.Length; i++)
		{
			result.SetValue(input[i], i);
		}

		return result;
	}
}