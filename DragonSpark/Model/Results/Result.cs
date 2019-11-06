using System;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Results
{
	public class Result<T> : IResult<T>, IActivateUsing<Func<T>>, IActivateUsing<IResult<T>>
	{
		readonly Func<T> _source;

		public Result(IResult<T> result) : this(result.Get) {}

		public Result(Func<T> source) => _source = source;

		public T Get() => _source();

		public static implicit operator T(Result<T> result) => result.Get();
	}
}