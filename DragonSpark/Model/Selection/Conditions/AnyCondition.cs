using DragonSpark.Compose;
using DragonSpark.Model.Sequences;
using System;
using System.Linq;

namespace DragonSpark.Model.Selection.Conditions;

public class AnyCondition<T> : ICondition<T>
{
	readonly Array<Func<T, bool>> _specifications;

	public AnyCondition(params ISelect<T, bool>[] conditions)
		: this(conditions.Select(x => x.ToDelegate()).Result()) {}

	public AnyCondition(params Func<T, bool>[] specifications) : this(specifications.Result()) {}

	public AnyCondition(Array<Func<T, bool>> specifications) => _specifications = specifications;

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