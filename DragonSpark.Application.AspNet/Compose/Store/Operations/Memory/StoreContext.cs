using DragonSpark.Model.Selection;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Compose.Store.Operations.Memory;

public class StoreContext<TIn, TOut>
{
	public StoreContext(ISelect<TIn, ValueTask<TOut>> subject, IMemoryCache memory)
	{
		Subject = subject;
		Memory  = memory;
	}

	protected ISelect<TIn, ValueTask<TOut>> Subject { get; }

	protected IMemoryCache Memory { get; }

	public ConfiguredStoreContext<TIn, TOut> UntilRemoved() => new(Subject, Memory);

	public ConfiguredStoreContext<TIn, TOut> For(TimeSpan duration)
		=> new(Subject, Memory, new RelativeExpiration(duration));

	public ConfiguredStoreContext<TIn, TOut> For(Slide duration)
		=> new(Subject, Memory, new SlidingExpiration(duration.For));
}