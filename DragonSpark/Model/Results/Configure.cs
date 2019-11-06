using System;

namespace DragonSpark.Model.Results
{
	sealed class Configure<TIn, TOut> : IResult<TOut>
	{
		readonly Action<TIn, TOut> _configuration;
		readonly Func<TIn>         _instance;
		readonly Func<TIn, TOut>   _select;

		public Configure(Func<TIn> instance, Func<TIn, TOut> select, Action<TIn, TOut> configuration)
		{
			_instance      = instance;
			_select        = select;
			_configuration = configuration;
		}

		public TOut Get()
		{
			var instance = _instance();
			var result   = _select(instance);
			_configuration(instance, result);
			return result;
		}
	}
}