using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Distributed;
using System;

namespace DragonSpark.Application.Compose.Store.Operations.Distributed;

public class RelativeExpiration : ICommand<DistributedCacheEntryOptions>
{
	readonly TimeSpan _valid;

	public RelativeExpiration(TimeSpan valid) => _valid = valid;

	public void Execute(DistributedCacheEntryOptions parameter)
	{
		parameter.SetAbsoluteExpiration(_valid);
	}
}