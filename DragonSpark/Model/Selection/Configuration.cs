using DragonSpark.Compose;
using DragonSpark.Model.Commands;
using System;

namespace DragonSpark.Model.Selection
{
	sealed class Configuration<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Action<TIn, TOut> _configuration;
		readonly Func<TIn, TOut>   _source;

		public Configuration(ISelect<TIn, TOut> select, IAssign<TIn, TOut> configuration)
			: this(select.Get, configuration.Assign) {}

		public Configuration(Func<TIn, TOut> source, Action<TIn, TOut> configuration)
		{
			_source        = source;
			_configuration = configuration;
		}

		public TOut Get(TIn parameter)
		{
			var result = _source(parameter);
			_configuration(parameter, result);
			return result;
		}
	}

	sealed class Configuration<TIn, TOut, TOther> : ISelect<TIn, TOut>
	{
		readonly Action<TIn, TOther> _configuration;
		readonly Func<TOut, TOther>  _other;
		readonly Func<TIn, TOut>     _source;

		public Configuration(ISelect<TIn, TOut> select, IAssign<TIn, TOther> configuration)
			: this(select, Start.A.Selection<TOut>().AndOf<TOther>().By.Cast, configuration) {}

		public Configuration(ISelect<TIn, TOut> select, ISelect<TOut, TOther> other, IAssign<TIn, TOther> configuration)
			: this(select.Get, other.Get, configuration.Assign) {}

		public Configuration(Func<TIn, TOut> source, Func<TOut, TOther> other, Action<TIn, TOther> configuration)
		{
			_source        = source;
			_other         = other;
			_configuration = configuration;
		}

		public TOut Get(TIn parameter)
		{
			var result = _source(parameter);
			_configuration(parameter, _other(result));
			return result;
		}
	}
}