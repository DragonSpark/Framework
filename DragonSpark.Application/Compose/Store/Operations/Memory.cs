using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Compose.Store.Operations
{
	sealed class Memory<TIn, TOut> : Accessing<TIn, TOut>
	{
		public Memory(IMemoryCache memory, Get<TIn, TOut> get, Func<TIn, object> key)
			: base(memory.TryGetValue, get, key) {}
	}
}