using DragonSpark.Model.Commands;
using Microsoft.Extensions.Caching.Memory;

namespace DragonSpark.Application.Compose.Store;

public class SlidingExpiration : ICommand<ICacheEntry>
{
	readonly TimeSpan _duration;

	public SlidingExpiration(TimeSpan duration) => _duration = duration;

	public void Execute(ICacheEntry parameter)
	{
		parameter.SetSlidingExpiration(_duration);
	}
}