using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Results;

sealed class Results<T> : Select<IResult<T>, T>
{
	public static Results<T> Default { get; } = new Results<T>();

	Results() : base(x => x.Get()) {}
}