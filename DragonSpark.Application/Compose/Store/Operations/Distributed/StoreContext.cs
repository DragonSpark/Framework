using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

public class StoreContext<TIn, TOut>
{
	public StoreContext(ISelect<TIn, ValueTask<TOut>> subject, IDistributedCache memory)
	{
		Subject = subject;
		Memory  = memory;
	}

	protected ISelect<TIn, ValueTask<TOut>> Subject { get; }

	protected IDistributedCache Memory { get; }

	public ConfiguredStoreContext<TIn, TOut> UntilRemoved()
		=> new(Subject, Memory, EmptyCommand<DistributedCacheEntryOptions>.Default);

	public ConfiguredStoreContext<TIn, TOut> For(TimeSpan duration)
		=> new(Subject, Memory, new RelativeExpiration(duration));

	public ConfiguredStoreContext<TIn, TOut> For(Slide duration)
		=> new(Subject, Memory, new SlidingExpiration(duration.For));
}
