using System;

namespace DragonSpark.Model.Selection
{
	public class Try<TException, TIn, TOut> : ISelect<TIn, TOut> where TException : Exception
	{
		readonly Func<TIn, TOut> _fallback;
		readonly Func<TIn, TOut> _source;

		public Try(Func<TIn, TOut> source) : this(source, Default<TIn, TOut>.Instance.Get) {}

		public Try(Func<TIn, TOut> source, Func<TIn, TOut> fallback)
		{
			_source   = source;
			_fallback = fallback;
		}

		public TOut Get(TIn parameter)
		{
			try
			{
				var source = _source(parameter);
				return source;
			}
			catch (TException)
			{
				return _fallback(parameter);
			}
		}
	}
}