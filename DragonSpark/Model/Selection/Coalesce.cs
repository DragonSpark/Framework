using System;

namespace DragonSpark.Model.Selection
{
	public class Coalesce<TIn, TOut> : ISelect<TIn, TOut> where TOut : class
	{
		readonly Func<TIn, TOut?> _first;
		readonly Func<TIn, TOut>  _second;

		public Coalesce(ISelect<TIn, TOut?> first, ISelect<TIn, TOut> second) : this(first.Get, second.Get) {}

		public Coalesce(Func<TIn, TOut?> first, Func<TIn, TOut> second)
		{
			_first  = first;
			_second = second;
		}

		public TOut Get(TIn parameter) => _first(parameter) ?? _second(parameter);
	}
}