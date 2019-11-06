using System;
using DragonSpark.Model.Results;

namespace DragonSpark.Model.Selection.Adapters
{
	public class ResultInstanceSelector<_, T> : Selector<_, IResult<T>>
	{
		public ResultInstanceSelector(ISelect<_, IResult<T>> subject) : base(subject) {}

		public Selector<_, T> Value() => Select(Results<T>.Default);

		public Selector<_, Func<T>> Delegate() => Select(DelegateSelector<T>.Default);
	}
}