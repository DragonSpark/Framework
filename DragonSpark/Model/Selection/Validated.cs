using DragonSpark.Model.Selection.Conditions;
using System;

namespace DragonSpark.Model.Selection
{
	public class Validated<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<TIn, TOut> _source, _fallback;
		readonly Func<TIn, bool> _specification;

		public Validated(ICondition<TIn> condition, ISelect<TIn, TOut> select)
			: this(condition, select, Default<TIn, TOut>.Instance) {}

		public Validated(ICondition<TIn> condition, ISelect<TIn, TOut> select, ISelect<TIn, TOut> fallback)
			: this(condition.Get, select.Get, fallback.Get) {}

		public Validated(Func<TIn, bool> specification, Func<TIn, TOut> source)
			: this(specification, source, Default<TIn, TOut>.Instance.Get) {}

		public Validated(Func<TIn, bool> specification, Func<TIn, TOut> source, Func<TIn, TOut> fallback)
		{
			_specification = specification;
			_source        = source;
			_fallback      = fallback;
		}

		public TOut Get(TIn parameter) => _specification(parameter) ? _source(parameter) : _fallback(parameter);
	}
}