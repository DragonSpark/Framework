using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Model.Sequences
{
	public class ArraySelection<_, T> : Select<_, Array<T>>, IArray<_, T>
	{
		public ArraySelection(ISelect<_, Array<T>> select) : base(select) {}

		public ArraySelection(Func<_, Array<T>> select) : base(select) {}
	}
}