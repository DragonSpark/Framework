using DragonSpark.Model;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Selection;
using DragonSpark.Runtime;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.Content.History;

public class Tracker<TIn, TOut> : ISelect<TIn, TOut?>, IAssign<TIn, TOut?>
{
	readonly Func<TIn, string>                                _key;
	readonly ConcurrentDictionary<string, HistoryEntry<TOut>> _entries;
	readonly byte                                             _size;
	readonly ITime                                            _time;

	protected Tracker(Func<TIn, string> key, byte size) : this(key, [], size, Time.Default) {}

	// ReSharper disable once TooManyDependencies
	protected Tracker(Func<TIn, string> key, ConcurrentDictionary<string, HistoryEntry<TOut>> entries, byte size,
	                  ITime time)
	{
		_key     = key;
		_entries = entries;
		_size    = size;
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
		if (_entries.Count > _size)
		{
			foreach (var item in _entries.Values.OrderBy(x => x.When).Take(_entries.Count - _size))
			{
				_entries.TryRemove(item.Key, out _);
			}
		}
	}
}