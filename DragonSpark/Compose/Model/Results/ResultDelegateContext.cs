using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Compose.Model.Results;

public class ResultDelegateContext<T> : ResultContext<Func<T>>
{
	public ResultDelegateContext(IResult<Func<T>> instance) : base(instance) {}

	public ResultContext<T> Assume() => new Assume<T>(this).Then();
}