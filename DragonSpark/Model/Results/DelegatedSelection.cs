using DragonSpark.Model.Selection;
using System;

namespace DragonSpark.Model.Results
{
	public class DelegatedSelection<TIn, TOut> : IResult<TOut>
	{
		readonly Func<TIn>       _parameter;
		readonly Func<TIn, TOut> _source;

		public DelegatedSelection(ISelect<TIn, TOut> select, IResult<TIn> parameter)
			: this(select.Get, parameter.Get) {}

		public DelegatedSelection(Func<TIn, TOut> source, Func<TIn> parameter)
		{
			_source    = source;
			_parameter = parameter;
		}

		public TOut Get() => _source(_parameter());
	}
}