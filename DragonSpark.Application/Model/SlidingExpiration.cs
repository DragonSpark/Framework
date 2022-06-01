using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace DragonSpark.Application.Model;

public class SlidingExpiration : ICommand<ICacheEntry>
{
	readonly TimeSpan _duration;

	public SlidingExpiration(TimeSpan duration) => _duration = duration;

	public void Execute(ICacheEntry parameter)
	{
		parameter.SetSlidingExpiration(_duration);
	}
}