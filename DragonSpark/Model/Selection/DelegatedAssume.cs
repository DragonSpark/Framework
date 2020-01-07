using DragonSpark.Model.Results;
using System;

namespace DragonSpark.Model.Selection
{
	public class DelegatedAssume<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<Func<TIn, TOut>> _source;

		public DelegatedAssume(Func<Func<TIn, TOut>> source) => _source = source;

		public TOut Get(TIn parameter) => _source()(parameter);
	}

	public class Assume<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly IResult<ISelect<TIn, TOut>> _source;

		public Assume(IResult<ISelect<TIn, TOut>> source) => _source = source;

		public TOut Get(TIn parameter) => _source.Get().Get(parameter);
	}
}