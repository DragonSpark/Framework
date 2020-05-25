using DragonSpark.Model.Operations;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	sealed class Source<TIn, TOut> : ISelecting<(TIn Parameter, object Key), TOut>
	{
		readonly IMemoryCache               _memory;
		readonly Func<TIn, ValueTask<TOut>> _source;
		readonly Action<ICacheEntry> _configure;

		public Source(IMemoryCache memory, Func<TIn, ValueTask<TOut>> source, Action<ICacheEntry> configure)
		{
			_memory    = memory;
			_source    = source;
			_configure = configure;
		}

		public async ValueTask<TOut> Get((TIn Parameter, object Key) parameter)
		{
			var (@in, key) = parameter;
			using var entry  = _memory.CreateEntry(key);
			var       result = await _source(@in);
			entry.Value = result;
			_configure(entry);
			return result;
		}
	}
}
