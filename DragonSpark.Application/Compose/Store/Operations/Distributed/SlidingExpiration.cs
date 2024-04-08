using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

public class SlidingExpiration : ICommand<DistributedCacheEntryOptions>
{
	readonly TimeSpan _duration;

	public SlidingExpiration(TimeSpan duration) => _duration = duration;

	public void Execute(DistributedCacheEntryOptions parameter)
	{
		parameter.SetSlidingExpiration(_duration);
	}
}