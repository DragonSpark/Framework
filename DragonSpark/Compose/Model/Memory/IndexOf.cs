using DragonSpark.Model.Selection;
using System;
using System.Collections.Generic;

namespace DragonSpark.Compose.Model.Memory;

sealed class IndexOf<T> : ISelect<(Memory<T> Subject, T Candidate, IEqualityComparer<T> Comparer), uint?>
{
	public static IndexOf<T> Default { get; } = new IndexOf<T>();

	IndexOf() {}

	public uint? Get((Memory<T> Subject, T Candidate, IEqualityComparer<T> Comparer) parameter)
	{
		var (subject, candidate, comparer) = parameter;

		var span = subject.Span;
		for (var i = 0; i < span.Length; i++)
		{
			if (comparer.Equals(span[i], candidate))
			{
				return (uint)i;
			}
		}

		return null;
	}
}