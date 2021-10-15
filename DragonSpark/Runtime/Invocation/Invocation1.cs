using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Invocation;

public class Invocation1<T1, T2, TOut> : ISelect<T2, TOut>
{
	readonly Func<T1, T2, TOut> _delegate;
	readonly T1                 _parameter;

	public Invocation1(Func<T1, T2, TOut> @delegate, T1 parameter)
	{
		_delegate  = @delegate;
		_parameter = parameter;
	}

	public TOut Get(T2 parameter) => _delegate(_parameter, parameter);
}