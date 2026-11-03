using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Runtime;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class RecentContentTracker : Tracker<Microsoft.AspNetCore.Components.ComponentBase, object?>
{
	public RecentContentTracker(IRenderContentKey key) : base(key.Get) {}
}

// TODO

sealed class RecentHeightTracker : Tracker<string, double?>
{
	public RecentHeightTracker() : base(s => s) {}
}

public class Tracker<TIn, TOut> : ISelect<TIn, TOut?>, IAssign<TIn, TOut?>
{
	readonly Func<TIn, string>                                _key;
	readonly ConcurrentDictionary<string, HistoryEntry<TOut>> _entries;
	readonly ITime                                            _time;

	protected Tracker(Func<TIn, string> key) : this(key, [], Time.Default) {}

	protected Tracker(Func<TIn, string> key, ConcurrentDictionary<string, HistoryEntry<TOut>> entries, ITime time)
	{
		_key     = key;
		_entries = entries;
		_time    = time;
	}

	public TOut? Get(TIn parameter)
	{
		var key = _key(parameter);
		if (_entries.TryGetValue(key, out var entry))
		{
			var updated = entry with { When = _time.Get() };
			_entries.TryUpdate(key, updated, entry);
			return entry.Result;
		}

		return default;
	}

	public void Execute(Pair<TIn, TOut?> parameter)
	{
		var (component, value) = parameter;
		var key = _key(component);
		var now = _time.Get();

		if (_entries.TryGetValue(key, out var entry))
		{
			_entries.TryUpdate(key, entry with { Result = value, When = now }, entry);
		}
		else
		{
			_entries.TryAdd(key, new(key, value, now));
		}

		EnforceCapacity();
	}

	void EnforceCapacity()
	{
		var limit = HistoryCount.Default;
		if (_entries.Count > limit)
		{
			foreach (var item in _entries.Values.OrderBy(x => x.When).Take(_entries.Count - limit))
			{
				_entries.TryRemove(item.Key, out _);
			}
		}
	}
}