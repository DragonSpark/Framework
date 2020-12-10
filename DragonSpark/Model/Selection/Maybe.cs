using System;

namespace DragonSpark.Model.Selection
{
	public class Maybe<TIn, TOut> : ISelect<TIn, TOut?> where TOut : class
	{
		readonly Func<TIn, TOut?> _first, _second;

		public Maybe(ISelect<TIn, TOut?> first, ISelect<TIn, TOut?> second) : this(first.Get, second.Get) {}

		public Maybe(Func<TIn, TOut?> first, Func<TIn, TOut?> second)
		{
			_first  = first;
			_second = second;
		}

		public TOut? Get(TIn parameter) => _first(parameter) ?? _second(parameter);
	}
}