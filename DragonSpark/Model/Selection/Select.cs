using System;
using DragonSpark.Runtime.Activation;

namespace DragonSpark.Model.Selection
{
	public class Select<TIn, TOut> : ISelect<TIn, TOut>,
	                                 IActivateUsing<Func<TIn, TOut>>
	{
		readonly Func<TIn, TOut> _source;

		public Select(ISelect<TIn, TOut> select) : this(select.Get) {}

		public Select(Func<TIn, TOut> select) => _source = select;

		public TOut Get(TIn parameter) => _source(parameter);
	}
}