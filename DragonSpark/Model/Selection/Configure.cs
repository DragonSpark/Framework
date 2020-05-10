using System;

namespace DragonSpark.Model.Selection
{
	sealed class Configure<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Action<TIn, TOut> _configure;
		readonly Func<TIn, TOut>   _select;

		public Configure(Func<TIn, TOut> select, Action<TIn, TOut> configure)
		{
			_select    = select;
			_configure = configure;
		}

		public TOut Get(TIn parameter)
		{
			var result = _select(parameter);
			_configure(parameter, result);
			return result;
		}
	}
}