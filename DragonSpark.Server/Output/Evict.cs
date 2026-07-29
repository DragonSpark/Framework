using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Operations.Stop;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.OutputCaching;

namespace DragonSpark.Server.Output;

// TODO

public readonly record struct EvictInput(object Input, object? Output = null);
public sealed class Evict : IStopAware<EvictInput>
{
	readonly IOutputCacheStore _store;
	readonly ITags             _tags;
	readonly Array<IOutputKey> _keys;

	public Evict(IOutputCacheStore store, Array<IOutputKey> keys) : this(store, Tags.Default, keys) {}

	public Evict(IOutputCacheStore store, ITags tags, Array<IOutputKey> keys)
	{
		_store = store;
		_tags  = tags;
		_keys  = keys;
	}

	public async ValueTask Get(Stop<EvictInput> parameter)
	{
		var ((input, output), stop) = parameter;

		var tags    = new List<string>();
		var current = new HashSet<string>();
		foreach (var key in _keys.Open())
		{
			await _tags.Off(new(new(input, key, current), stop));
			if (output is not null)
			{
				await _tags.Off(new(new(output, key, current), stop));	
			}
			tags.AddRange(current);
			current.Clear();
		}

		foreach (var tag in tags)
		{
			await _store.EvictByTagAsync(tag, stop).Off();
		}
	}
}

sealed class Evict<TIn, T> : IStopAware<TIn, T> where TIn : notnull
{
	readonly ISelecting<Stop<TIn>, T> _previous;
	readonly IStopAware<EvictInput>   _evict;

	public Evict(IStopAware<TIn, T> previous, IOutputCacheStore output, params IOutputKey[] keys)
		: this(previous, new Evict(output, keys)) {}

	public Evict(IStopAware<TIn, T> previous, IStopAware<EvictInput> evict)
	{
		_previous = previous;
		_evict    = evict;
	}

	public async ValueTask<T> Get(Stop<TIn> parameter)
	{
		var (subject, stop) = parameter;
		var result       = await _previous.Off(parameter);
		await _evict.Off(new(new(subject, result), stop));
		return result;
	}
}