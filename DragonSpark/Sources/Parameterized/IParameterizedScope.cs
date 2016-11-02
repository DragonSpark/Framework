using System;
using DragonSpark.Sources.Scopes;

namespace DragonSpark.Sources.Parameterized
{
	public interface IParameterizedScope<T> : IParameterizedScope<object, T>, IParameterizedSource<T> {}

	public interface IParameterizedScope<TParameter, TResult> : IParameterizedSource<TParameter, TResult>, IScopeAware<Func<TParameter, TResult>>
	{
		Func<TParameter, TResult> GetFactory();
	}
}