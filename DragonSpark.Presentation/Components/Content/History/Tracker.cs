using DragonSpark.Model;
using DragonSpark.Runtime;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.Content.History;

public class Tracker<TIn, TOut> : Tracker<TIn, string, TOut>
{
	protected Tracker(Func<TIn, string> key, byte size) : base(key, size) {}

	// ReSharper disable once TooManyDependencies
	protected Tracker(Func<TIn, string> key, ConcurrentDictionary<string, HistoryEntry<TOut>> entries, byte size,
	                  ITime time)
		: base(key, entries, size, time) {}
}

public class Tracker<TIn, TKey, TOut> : ITracker<TIn, TOut> where TKey : notnull
{
	readonly Func<TIn, TKey>                                _key;
	readonly ConcurrentDictionary<TKey, HistoryEntry<TOut>> _entries;
	readonly byte                                           _size;
	readonly ITime                                          _time;

	protected Tracker(Func<TIn, TKey> key, byte size) : this(key, [], size, Time.Default) {}

	// ReSharper disable once TooManyDependencies
	protected Tracker(Func<TIn, TKey> key, ConcurrentDictionary<TKey, HistoryEntry<TOut>> entries, byte size,
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
		var key  = _key(component);
		var now  = _time.Get();
		var next = new HistoryEntry<TOut>(value, now);
		if (_entries.TryGetValue(key, out var entry))
		{
			_entries.TryUpdate(key, next, entry);
		}
		else
		{
			_entries.TryAdd(key, next);
		}

		EnforceCapacity();
	}

	void EnforceCapacity()
	{
		if (_entries.Count > _size)
		{
			foreach (var item in _entries.OrderBy(x => x.Value.When).Take(_entries.Count - _size))
			{
				_entries.TryRemove(item.Key, out _);
			}
		}
	}
}