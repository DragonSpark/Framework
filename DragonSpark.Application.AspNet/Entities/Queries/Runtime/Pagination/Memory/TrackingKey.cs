using DragonSpark.Text;
using System.Collections.Concurrent;

namespace DragonSpark.Application.AspNet.Entities.Queries.Runtime.Pagination.Memory;

public class TrackingKey<T> : IFormatter<T>
{
	readonly IFormatter<T>         _previous;
	readonly ConcurrentBag<string> _track;

	protected TrackingKey(IFormatter<T> previous, ConcurrentBag<string> track)
	{
		_previous = previous;
		_track    = track;
	}

	public string Get(T parameter)
	{
		var result = _previous.Get(parameter);
		_track.Add(result);
		return result;
	}
}