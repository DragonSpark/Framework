using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model.Selection
{
	class ValidatedResult<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<TIn, TOut>  _fallback;
		readonly Func<TIn, TOut>  _source;
		readonly Func<TOut, bool> _specification;

		public ValidatedResult(ICondition<TOut> condition, ISelect<TIn, TOut> select, ISelect<TIn, TOut> fallback)
			: this(condition.Get, select.Get, fallback.Get) {}

		public ValidatedResult(Func<TOut, bool> specification, Func<TIn, TOut> source, Func<TIn, TOut> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public TOut Get(TIn parameter)
		{
			var candidate = _source(parameter);
			var result    = _specification(candidate) ? candidate : _fallback(parameter);
			return result;
		}
	}
}