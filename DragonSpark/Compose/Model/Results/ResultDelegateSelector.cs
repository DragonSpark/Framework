using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Invocation;
using System;

namespace DragonSpark.Compose.Model.Results;

public class ResultDelegateSelector<_, T> : Selector<_, Func<T>>
{
	public ResultDelegateSelector(ISelect<_, Func<T>> subject) : base(subject) {}

	public Selector<_, Func<T>> Singleton() => Select(SingletonDelegate<T>.Default);

	public Selector<_, T> Invoke() => Select(Call<T>.Default);
}