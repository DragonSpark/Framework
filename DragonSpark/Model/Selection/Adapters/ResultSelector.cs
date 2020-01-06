using DragonSpark.Compose;
using DragonSpark.Runtime;
using System;

namespace DragonSpark.Model.Selection.Adapters
{
	public class ResultSelector<T> : Selector<None, T>
	{
		public static implicit operator Func<T>(ResultSelector<T> instance) => instance.Get().ToResult().ToDelegate();

		public ResultSelector(ISelect<None, T> subject) : base(subject) {}

		public Selector<TIn, T> Accept<TIn>() => Start.A.Selection<TIn>().By.Returning(Get().ToResult()).Then();
	}
}