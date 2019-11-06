using System;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Results
{
	sealed class DelegateSelector<T> : ISelect<IResult<T>, Func<T>>
	{
		public static ISelect<IResult<T>, Func<T>> Default { get; } = new DelegateSelector<T>();

		DelegateSelector() {}

		public Func<T> Get(IResult<T> parameter) => parameter.Get;
	}
}