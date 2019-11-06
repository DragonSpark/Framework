using System;
using System.Collections.Immutable;
using System.Linq;

namespace DragonSpark.Model.Selection.Conditions
{
	public class AllCondition<T> : ICondition<T>
	{
		readonly ImmutableArray<Func<T, bool>> _specifications;

		public AllCondition(params ISelect<T, bool>[] conditions)
			: this(conditions.Select(x => x.ToDelegate()).ToImmutableArray()) {}

		public AllCondition(params Func<T, bool>[] conditions) : this(conditions.ToImmutableArray()) {}

		public AllCondition(ImmutableArray<Func<T, bool>> specifications)
			=> _specifications = specifications;

		public bool Get(T parameter)
		{
			var length = _specifications.Length;
			for (var i = 0; i < length; i++)
			{
				if (!_specifications[i](parameter))
				{
					return false;
				}
			}

			return true;
		}
	}
}