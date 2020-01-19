using DragonSpark.Model.Selection;
using DragonSpark.Runtime.Activation;
using System;

namespace DragonSpark.Runtime.Invocation
{
	sealed class Protect<TIn, TOut> : ISelect<TIn, TOut>, IActivateUsing<ISelect<TIn, TOut>>
	{
		readonly Func<TIn, TOut> _source;

		public Protect(ISelect<TIn, TOut> select) : this(select.Get) {}

		public Protect(Func<TIn, TOut> source) => _source = source;

		public TOut Get(TIn parameter)
		{
			lock (_source)
			{
				return _source(parameter);
			}
		}
	}
}