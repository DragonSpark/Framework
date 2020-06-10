using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DragonSpark.Model.Sequences.Collections
{
	public class DelegatedComparer<T> : IComparer<T>
	{
		readonly Func<T, int> _sort;

		public DelegatedComparer(Func<T, int> select) => _sort = select;

		public int Compare([AllowNull]T x, [AllowNull]T y) => _sort(x!).CompareTo(_sort(y!));
	}
}