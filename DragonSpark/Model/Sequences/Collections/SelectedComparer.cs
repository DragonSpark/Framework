using System;
using System.Collections.Generic;

namespace DragonSpark.Model.Sequences.Collections
{
	public class SelectedComparer<T, TMember> : IComparer<T>
	{
		readonly IComparer<TMember> _member;
		readonly Func<T, TMember>   _select;

		public SelectedComparer(Func<T, TMember> select) : this(select, SortComparer<TMember>.Default) {}

		public SelectedComparer(Func<T, TMember> select, IComparer<TMember> member)
		{
			_select = select;
			_member = member;
		}

		public int Compare(T x, T y) => _member.Compare(_select(x), _select(x));
	}
}