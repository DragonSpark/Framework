using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Runtime.Invocation
{
	public class Invocation0<T1, T2, TOut> : ISelect<T1, TOut>
	{
		readonly Func<T1, T2, TOut> _delegate;
		readonly T2                 _parameter;

		public Invocation0(Func<T1, T2, TOut> @delegate, T2 parameter)
		{
			_delegate  = @delegate;
			_parameter = parameter;
		}

		public TOut Get(T1 parameter) => _delegate(parameter, _parameter);
	}
}