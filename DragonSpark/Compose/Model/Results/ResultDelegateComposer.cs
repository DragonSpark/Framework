using DragonSpark.Compose.Model.Selection;
using DragonSpark.Model.Results;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Runtime.Invocation;
using System;

namespace DragonSpark.Compose.Model.Results;

public class ResultDelegateComposer<T> : ResultComposer<Func<T>>
{
	public ResultDelegateComposer(IResult<Func<T>> instance) : base(instance) {}

	public ResultComposer<T> Assume() => new Assume<T>(this).Then();
}

public class ResultDelegateComposer<_, T> : Composer<_, Func<T>>
{
	public ResultDelegateComposer(ISelect<_, Func<T>> subject) : base(subject) {}

	public Composer<_, Func<T>> Singleton() => Select(SingletonDelegate<T>.Default);

	public Composer<_, T> Invoke() => Select(Call<T>.Default);
}