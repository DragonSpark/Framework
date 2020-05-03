using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store
{
	sealed class Source<TIn, TOut> : ISelect<(TIn Parameter, object Key), TOut>
	{
		readonly IMemoryCache               _memory;
		readonly Func<TIn, TOut>            _source;
		readonly Action<ICacheEntry> _configure;

		public Source(IMemoryCache memory, Func<TIn, TOut> source, Action<ICacheEntry> configure)
		{
			_memory    = memory;
			_source    = source;
			_configure = configure;
		}

		public TOut Get((TIn Parameter, object Key) parameter)
		{
			var (@in, key) = parameter;
			using var entry  = _memory.CreateEntry(key);
			var       result = _source(@in);
			entry.Value = result;
			_configure(entry);
			return result;
		}
	}
}