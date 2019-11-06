using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Model.Selection.Conditions
{
	public class AnyCondition<T> : ICondition<T>
	{
		readonly ImmutableArray<Func<T, bool>> _specifications;

		public AnyCondition(params ISelect<T, bool>[] conditions)
			: this(conditions.Select(x => x.ToDelegate()).ToImmutableArray()) {}

		public AnyCondition(params Func<T, bool>[] specifications) : this(specifications.ToImmutableArray()) {}

		public AnyCondition(ImmutableArray<Func<T, bool>> specifications) => _specifications = specifications;

		public bool Get(T parameter)
		{
			var length = _specifications.Length;
			for (var i = 0; i < length; i++)
			{
				if (_specifications[i](parameter))
				{
					return true;
				}
			}

			return false;
		}
	}
}