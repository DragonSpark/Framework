using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Compose.Model.Results;

public class ResultSelectionComposer<_, T> : Composer<_, IResult<T>>
{
	public ResultSelectionComposer(ISelect<_, IResult<T>> subject) : base(subject) {}

	public Composer<_, T> Value() => Select(Results<T>.Default);

	public Composer<_, Func<T>> Delegate() => Select(DelegateSelector<T>.Default);
}