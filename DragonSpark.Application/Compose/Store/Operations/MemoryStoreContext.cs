using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations
{
	public class MemoryStoreContext<TIn, TOut>
	{
		public MemoryStoreContext(ISelect<TIn, ValueTask<TOut>> subject, IMemoryCache memory)
		{
			Subject = subject;
			Memory  = memory;
		}

		protected ISelect<TIn, ValueTask<TOut>> Subject { get; }

		protected IMemoryCache Memory { get; }

		public ConfiguredMemoryStoreContext<TIn, TOut> UntilRemoved()
			=> new(Subject, Memory, EmptyCommand<ICacheEntry>.Default);

		public ConfiguredMemoryStoreContext<TIn, TOut> For(TimeSpan duration)
			=> new(Subject, Memory, new RelativeExpiration(duration));

		public ConfiguredMemoryStoreContext<TIn, TOut> For(Slide duration)
			=> new(Subject, Memory, new SlidingExpiration(duration.For));
	}
}