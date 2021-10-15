using DragonSpark.Model.Selection.Conditions;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose.Model.Memory;

sealed class Contains<T> : ICondition<(Memory<T> Subject, T Candidate, IEqualityComparer<T> Comparer)>
{
	public static Contains<T> Default { get; } = new Contains<T>();

	Contains() {}

	public bool Get((Memory<T> Subject, T Candidate, IEqualityComparer<T> Comparer) parameter)
	{
		var (subject, candidate, comparer) = parameter;

		var span = subject.Span;
		for (var i = 0; i < span.Length; i++)
		{
			if (comparer.Equals(span[i], candidate))
			{
				return true;
			}
		}

		return false;
	}
}