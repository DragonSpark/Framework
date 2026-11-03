using DragonSpark.Composition;
using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Presentation.Components.Content.Rendering;
using DragonSpark.Runtime;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class HistoryTracker : ISelect<object, object?>, IAssign<object, object?>
{
	readonly IRenderContentKey                          _key;
	readonly ConcurrentDictionary<string, HistoryEntry> _entries;
	readonly ITime                                      _time;

	public HistoryTracker(IRenderContentKey key) : this(key, [], Time.Default) {}

	[Candidate(false)]
	public HistoryTracker(IRenderContentKey key, ConcurrentDictionary<string, HistoryEntry> entries, ITime time)
	{
		_key     = key;
		_entries = entries;
		_time    = time;
	}

	public object? Get(object parameter)
	{
		var key = _key.Get(parameter);
		if (_entries.TryGetValue(key, out var entry))
		{
			var updated = entry with { When = _time.Get() };
			_entries.TryUpdate(key, updated, entry);
			return entry.Result;
		}

		return null;
	}

	public void Execute(Pair<object, object?> parameter)
	{
		var (component, value) = parameter;
		var key = _key.Get(component);
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