using System;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Conditions;

namespace DragonSpark.Model.Sequences.Query
{
	public class FirstWhere<T> : IReduce<T>
	{
		readonly Func<T>       _default;
		readonly Func<T, bool> _where;

		public FirstWhere(ICondition<T> where) : this(where.Get) {}

		public FirstWhere(Func<T, bool> where) : this(where, Default<T>.Instance.Get) {}

		public FirstWhere(Func<T, bool> where, Func<T> @default)
		{
			_where   = where;
			_default = @default;
		}

		public T Get(Store<T> parameter)
		{
			var length = parameter.Length;
			for (var i = 0u; i < length; i++)
			{
				var item = parameter.Instance[i];
				if (_where(item))
				{
					return item;
				}
			}

			return _default();
		}
	}
}