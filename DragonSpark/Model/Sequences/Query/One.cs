using System;
using DragonSpark.Compose;

namespace DragonSpark.Model.Sequences.Query
{
	public class One<T> : IReduce<T>
	{
		readonly Func<T>                          _default;
		readonly Func<ArrayView<T>, ArrayView<T>> _where;

		public One(Func<T, bool> where) : this(where, A.Default<T>) {}

		public One(Func<T, bool> where, Func<T> @default)
			: this(new Where<T>(where, Selection.Default, 2).Get, @default) {}

		public One(Func<ArrayView<T>, ArrayView<T>> where, Func<T> @default)
		{
			_where   = where;
			_default = @default;
		}

		public T Get(Store<T> parameter)
		{
			var view   = _where(new ArrayView<T>(parameter.Instance, 0, parameter.Length));
			var result = view.Length == 1 ? view.Array[0] : _default();
			return result;
		}
	}
}