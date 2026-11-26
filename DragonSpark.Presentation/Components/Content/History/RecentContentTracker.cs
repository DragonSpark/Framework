using DragonSpark.Model;
using DragonSpark.Runtime;
using System.Collections.Concurrent;

namespace DragonSpark.Presentation.Components.Content.History;

sealed class RecentContentTracker<T> : IRecentContentTracker<T>
{
	readonly IComponentKey                               _key;
	readonly ConcurrentDictionary<int, HistoryEntry<T?>> _entries;
	readonly byte                                        _size;
	readonly ITime                                       _time;

	public RecentContentTracker(IComponentKey key) : this(key, [], HistoryCount.Default, Time.Default) {}

	// ReSharper disable once TooManyDependencies
	public RecentContentTracker(IComponentKey key, ConcurrentDictionary<int, HistoryEntry<T?>> entries, byte size,
	                            ITime time)
	{
		_key     = key;
		_entries = entries;
		_size    = size;
		_time    = time;
	}

	public void Execute(Pair<ComponentEntryKey, T?> parameter)
	{
		var (component, value) = parameter;
		var key  = _key.Get(component);
		var now  = _time.Get();
		var next = new HistoryEntry<T?>(value, now);
		if (_entries.TryGetValue(key, out var entry))
		{
			_entries.TryUpdate(key, next, entry);
		}
		else
		{
			_entries.TryAdd(key, next);
		}

		EnforceCapacity();
		_key.Execute(component);
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

	public bool TryPop(ComponentKeyInput parameter, out ExistingContentResult<T> result)
	{
		var (subject, _) = parameter;
		var (path, hash) = _key.Get(parameter);
		var key = new ComponentEntryKey(subject, path);
		if (_entries.TryRemove(hash, out var entry))
		{
			result = new(key, entry.Result);
			return true;
		}

		result = new(key, default);
		return false;
	}
}