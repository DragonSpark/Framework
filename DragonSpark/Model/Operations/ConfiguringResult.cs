﻿using DragonSpark.Compose;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations
{
	public class ConfiguringResult<TSource, TResult> : ISelecting<TSource, TResult>
	{
		readonly Await<TSource, TResult> _select;
		readonly Await<TResult>          _configure;

		public ConfiguringResult(ISelecting<TSource, TResult> select, IOperation<TResult> operation)
			: this(select.Await, operation.Await) {}

		public ConfiguringResult(Await<TSource, TResult> select, Await<TResult> configure)
		{
			_select    = select;
			_configure = configure;
		}

		public async ValueTask<TResult> Get(TSource parameter)
		{
			var result = await _select(parameter);
			await _configure(result);
			return result;
		}
	}
}