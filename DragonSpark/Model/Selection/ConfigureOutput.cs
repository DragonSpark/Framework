using System;

namespace DragonSpark.Model.Selection;

sealed class ConfigureOutput<TIn, TOut> : ISelect<TIn, TOut>
{
	readonly Action<TOut>    _configure;
	readonly Func<TIn, TOut> _select;

	public ConfigureOutput(Func<TIn, TOut> select, Action<TOut> configure)
	{
		_select    = select;
		_configure = configure;
	}

	public TOut Get(TIn parameter)
	{
		var result = _select(parameter);
		_configure(result);
		return result;
	}
}