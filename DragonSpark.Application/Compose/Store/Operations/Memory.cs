using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	sealed class Memory<TIn, TOut> : IOperationResult<TIn, TOut>
	{
		readonly IMemoryCache      _memory;
		readonly Get<TIn, TOut>    _get;
		readonly Func<TIn, object> _key;

		public Memory(IMemoryCache memory, Get<TIn, TOut> get, Func<TIn, object> key)
		{
			_memory = memory;
			_get    = get;
			_key    = key;
		}

		public async ValueTask<TOut> Get(TIn parameter)
		{
			var key = _key(parameter);
			var result = _memory.TryGetValue(key, out var stored) && !(stored is null)
				             ? stored.To<TOut>()
				             : await _get((parameter, key));
			return result;
		}
	}
}